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
        public async Task SendTrainingCompletedEmailAsync(string email, string modelName)
        {
            Console.WriteLine($"Email has sent to {email} for model {modelName}");
            await Task.CompletedTask;
        }

        public async Task SendTrainingFailedEmailAsync(string email, string modelName, string error)
        {
            Console.WriteLine($"Email has sent to {email} for model {modelName} wiht error {error}");
            await Task.CompletedTask;
        }
    }
}
