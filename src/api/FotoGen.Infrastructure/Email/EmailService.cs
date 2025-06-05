using Azure;
using Azure.Communication.Email;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Entities.Emails;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSettings _settings;
    private readonly EmailClient _emailClient;

    public EmailService(
        IOptions<EmailSettings> settings,
        ILogger<EmailService> logger,
        EmailClient emailClient)
    {
        _settings = settings.Value;
        _logger = logger;
        _emailClient = emailClient;
    }

    public async Task SendTrainingCompletedEmailAsync(string email, string modelName, string appUrl, string shareUrl)
    {
        var template = EmailTemplates.GetTrainingCompletedTemplate(modelName, appUrl, shareUrl);
        await SendEmailAsync(email, template);
    }

    public async Task SendTrainingFailedEmailAsync(string email, string modelName, string error)
    {
        var template = EmailTemplates.GetTrainingFailedTemplate(modelName, error);
        await SendEmailAsync(email, template);
    }

    private async Task SendEmailAsync(string recipientEmail, EmailTemplate template)
    {
        try
        {
            var emailContent = new EmailContent(template.Subject)
            {
                PlainText = template.PlainText, Html = template.Html
            };

            var emailMessage = new EmailMessage(
                senderAddress: _settings.SenderEmail,
                recipientAddress: recipientEmail,
                content: emailContent);

            var emailSendOperation = await _emailClient.SendAsync(
                WaitUntil.Completed,
                emailMessage);

            if (emailSendOperation.HasValue)
            {
                _logger.LogDebug(
                    "Email sent successfully to {Recipient}. Status: {Status}, OperationId: {OperationId}",
                    recipientEmail,
                    emailSendOperation.Value.Status,
                    emailSendOperation.Id);
            }
        }
        catch (RequestFailedException ex)
        {
            _logger.LogError(
                ex,
                "Failed to send email to {Recipient}. ErrorCode: {ErrorCode}",
                recipientEmail,
                ex.ErrorCode);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unexpected error sending email to {Recipient}",
                recipientEmail);
            throw;
        }
    }
}
