using FotoGen.Application.Interfaces;
using FotoGen.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;

namespace FotoGen.Application.Events;

public class ModelTrainingSucceededEventHandler : INotificationHandler<ModelTrainingSucceededEvent>
{
    private readonly IEmailService _emailService;
    private readonly AppSettings _appSettings;
    private readonly ILogger<ModelTrainingSucceededEventHandler> _logger;

    public ModelTrainingSucceededEventHandler(
        IOptions<AppSettings> appSettings,
        IEmailService emailService,
        ILogger<ModelTrainingSucceededEventHandler> logger)
    {
        _appSettings = appSettings.Value;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(ModelTrainingSucceededEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Model training completed: {notification.Id}");
        var applink = _appSettings.Host;
        string sharedLink = $"{applink}/model/{notification.UserName}/{notification.ModelName}";
        await _emailService.SendTrainingCompletedEmailAsync(
            notification.UserEmail,
            notification.Model,
            applink,
            sharedLink);
    }
}
