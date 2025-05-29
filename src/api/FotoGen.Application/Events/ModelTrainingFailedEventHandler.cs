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
        _logger.LogError($"Model training failed: {notification.Model}. Error: {notification.Error}");
        await _emailService.SendTrainingFailedEmailAsync(
            notification.UserEmail,
            notification.Model,
            notification.Error);
    }
}
