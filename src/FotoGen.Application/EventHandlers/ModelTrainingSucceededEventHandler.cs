using FotoGen.Application.Interfaces;
using FotoGen.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FotoGen.Application.EventHandlers
{
    public class ModelTrainingSucceededEventHandler : INotificationHandler<ModelTrainingSucceededEvent>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<ModelTrainingSucceededEventHandler> _logger;

        public ModelTrainingSucceededEventHandler(
            IEmailService emailService,
            ILogger<ModelTrainingSucceededEventHandler> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Handle(ModelTrainingSucceededEvent notification, CancellationToken cancellationToken)
        {
            var link = "";
            //TODO: Need to make a link here
            _logger.LogInformation($"Model training completed: {notification.Id}");
            await _emailService.SendTrainingCompletedEmailAsync(
                notification.UserEmail,
                notification.Model,
                link);
        }
    }

}
