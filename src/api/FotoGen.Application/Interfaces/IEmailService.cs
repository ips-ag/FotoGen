namespace FotoGen.Application.Interfaces;

public interface IEmailService
{
    Task SendTrainingCompletedEmailAsync(string email, string modelName, string appUrl, string shareUrl);
    Task SendTrainingFailedEmailAsync(string email, string modelName, string error);
}
