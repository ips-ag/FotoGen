using FotoGen.Application.Events;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Interfaces;
using FotoGen.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FotoGen.Infrastructure.BackgroundServices;

public class ModelTrainingBackgroundService : BackgroundService
{
    private const int MaxConsecutiveErrors = 5;
    private readonly ILogger<ModelTrainingBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

    public ModelTrainingBackgroundService(
        ILogger<ModelTrainingBackgroundService> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Model Training Background Service is starting");

        var consecutiveErrors = 0;

        while (!stoppingToken.IsCancellationRequested && consecutiveErrors < MaxConsecutiveErrors)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var modelRepository = scope.ServiceProvider.GetRequiredService<ITrainedModelRepository>();
                var replicateClient = scope.ServiceProvider.GetRequiredService<IReplicateService>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var modelsInTraining = await modelRepository.GetByStatusAsync(TrainModelStatus.InProgress);

                if (modelsInTraining.Count == 0)
                {
                    _logger.LogDebug("No models currently in training status");
                    await Task.Delay(_checkInterval, stoppingToken);
                    continue;
                }

                _logger.LogInformation("Checking status for {ModelCount} models in training", modelsInTraining.Count);

              foreach (var trainedModel in modelsInTraining)
                {
                    try
                    {
                        var statusResult = await replicateClient.GetTrainModelStatusAsync(trainedModel.Id);

                        if (!statusResult.IsSuccess)
                        {
                            _logger.LogWarning(
                                "Failed to get status for model {ModelId}: {Error}",
                                trainedModel.Id,
                                statusResult.ErrorCode);
                            continue;
                        }
                        var trainedModelResult = statusResult.Data;
                        if (string.IsNullOrEmpty(trainedModelResult?.Status))
                        {
                            _logger.LogWarning("Empty status received for model {ModelId}", trainedModel.Id);
                            continue;
                        }

                        if (!Enum.TryParse<TrainModelStatus>(
                                trainedModelResult.Status,
                                ignoreCase: true,
                                out var newStatus) ||
                            !Enum.IsDefined(newStatus))
                        {
                            _logger.LogWarning(
                                "Invalid status '{Status}' received for model {ModelId}",
                                trainedModelResult?.Status,
                                trainedModel.Id);
                            continue;
                        }
                        if (newStatus == trainedModel.Status) continue;
                        trainedModel.Status = newStatus;
                        trainedModel.SuccessedAt = newStatus == TrainModelStatus.Succeeded ? DateTime.UtcNow : null;
                        await modelRepository.UpdateAsync(trainedModel);

                        if (newStatus == TrainModelStatus.Succeeded)
                        {
                            await mediator.Publish(
                                new ModelTrainingSucceededEvent(
                                    trainedModelResult.Id,
                                    trainedModelResult.Model,
                                    trainedModelResult.Version,
                                    trainedModel.UserEmail,
                                    trainedModel.UserName,
                                    trainedModel.ModelName),
                                stoppingToken);
                        }
                        else if (newStatus == TrainModelStatus.Failed)
                        {
                            await mediator.Publish(
                                new ModelTrainingFailedEvent(
                                    trainedModelResult.Id,
                                    trainedModelResult.Model,
                                    trainedModelResult.Version,
                                    trainedModel.UserEmail,
                                    statusResult.ErrorCode.ToString() ??
                                    "Training failed"),
                                stoppingToken);
                        }

                        _logger.LogInformation(
                            "Model {ModelId} status updated to {Status}",
                            trainedModel.Id,
                            newStatus);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error checking status for model {ModelId}", trainedModel.Id);
                    }
                }

                consecutiveErrors = 0;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                consecutiveErrors++;
                _logger.LogError(
                    ex,
                    "Error in Model Training Background Service (Consecutive errors: {ErrorCount})",
                    consecutiveErrors);

                if (consecutiveErrors >= MaxConsecutiveErrors)
                {
                    _logger.LogCritical(
                        "Reached maximum consecutive errors ({MaxErrors}). Stopping service.",
                        MaxConsecutiveErrors);
                    break;
                }
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("Model Training Background Service is stopping");
    }
}
