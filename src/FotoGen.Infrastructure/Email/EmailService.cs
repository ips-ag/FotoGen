using FotoGen.Application.Interfaces;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _settings;
        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }
        public Task SendTrainingCompletedEmailAsync(string email, string modelName)
        {
            throw new NotImplementedException();
        }

        public Task SendTrainingFailedEmailAsync(string email, string modelName, string error)
        {
            throw new NotImplementedException();
        }
    }
}
