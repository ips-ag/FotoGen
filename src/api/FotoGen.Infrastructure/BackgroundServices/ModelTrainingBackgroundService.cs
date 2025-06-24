using FotoGen.Application.Events;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Models;
using FotoGen.Domain.Repositories;
using FotoGen.Infrastructure.Replicate.GetTrainedModelStatus.Converters;
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
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);

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

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var modelRepository = scope.ServiceProvider.GetRequiredService<IModelTrainingRepository>();
                var replicateClient = scope.ServiceProvider.GetRequiredService<IReplicateService>();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var statusConverter = scope.ServiceProvider.GetRequiredService<TrainingStatusConverter>();
                var modelsInTraining = await modelRepository.GetByStatusAsync(
                    ModelTrainingStatus.InProgress,
                    stoppingToken);
                if (modelsInTraining.Count == 0)
                {
                    _logger.LogDebug("No models currently in training status");
                    await Task.Delay(_checkInterval, stoppingToken);
                    continue;
                }
                _logger.LogDebug("Checking status for {ModelCount} models in training", modelsInTraining.Count);
                foreach (var modelTraining in modelsInTraining)
                {
                    try
                    {
                        var statusResult =
                            await replicateClient.GetModelTrainingStatusAsync(modelTraining.Id, stoppingToken);
                        if (!statusResult.IsSuccess)
                        {
                            _logger.LogWarning(
                                "Failed to get status for model {ModelId}: {Error}",
                                modelTraining.Id,
                                statusResult.ErrorCode);
                            continue;
                        }
                        var trainedModelResult = statusResult.Data;
                        var newStatus = statusConverter.ToDomain(trainedModelResult.Status);
                        if (newStatus is null)
                        {
                            _logger.LogWarning(
                                "Empty or invalid status {ModelTrainingStatus} received for model {ModelId}",
                                trainedModelResult.Status,
                                modelTraining.ModelName);
                            continue;
                        }
                        if (newStatus == modelTraining.TrainingStatus) continue;
                        var updatedModelTraining = modelTraining with
                        {
                            TrainingStatus = newStatus.Value,
                            SucceededAt = newStatus == ModelTrainingStatus.Succeeded ? DateTime.UtcNow : null
                        };
                        await modelRepository.UpdateAsync(updatedModelTraining, stoppingToken);
                        if (newStatus == ModelTrainingStatus.Succeeded)
                        {
                            await mediator.Publish(
                                new ModelTrainingSucceededEvent(
                                    userEmail: modelTraining.UserEmail,
                                    modelName: modelTraining.ModelName,
                                    userName: modelTraining.UserName),
                                stoppingToken);
                        }
                        else if (newStatus == ModelTrainingStatus.Failed)
                        {
                            await mediator.Publish(
                                new ModelTrainingFailedEvent(
                                    userEmail: modelTraining.UserEmail,
                                    modelName: modelTraining.ModelName,
                                    error: statusResult.ErrorCode.ToString() ?? "Training failed"),
                                stoppingToken);
                        }
                        _logger.LogDebug(
                            "Model {ModelId} status updated to {Status}",
                            modelTraining.Id,
                            newStatus);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error checking status for model {ModelId}", modelTraining.Id);
                    }
                }
                consecutiveErrors = 0;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                consecutiveErrors++;
                if (consecutiveErrors >= MaxConsecutiveErrors)
                {
                    _logger.LogCritical(
                        "Reached maximum consecutive errors ({MaxErrors}). Stopping service",
                        MaxConsecutiveErrors);
                    break;
                }
                _logger.LogError(
                    ex,
                    "Error in Model Training Background Service (Consecutive errors: {ErrorCount})",
                    consecutiveErrors);
            }
            await Task.Delay(_checkInterval, stoppingToken);
        }
        _logger.LogInformation("Model Training Background Service is stopping");
    }
}
