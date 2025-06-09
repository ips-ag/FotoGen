using FotoGen.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FotoGen.Application.Events;

public class ModelTrainingFailedEventHandler : INotificationHandler<ModelTrainingFailedEvent>
{
    private readonly IEmailService _emailService;
    private readonly ILogger<ModelTrainingFailedEventHandler> _logger;

    public ModelTrainingFailedEventHandler(
        IEmailService emailService,
        ILogger<ModelTrainingFailedEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(ModelTrainingFailedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogError(
            "Model training failed: {ModelName}. Error: {Error}",
            notification.ModelName,
            notification.Error);
        await _emailService.SendTrainingFailedEmailAsync(
            email: notification.UserEmail,
            modelName: notification.ModelName,
            error: notification.Error);
    }
}
