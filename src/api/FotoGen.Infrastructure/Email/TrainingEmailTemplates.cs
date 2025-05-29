using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FotoGen.Domain.Entities.Models;

namespace FotoGen.Infrastructure.Email
{
    public static class TrainingEmailTemplates
    {
        public static EmailTemplate GetTrainingCompletedTemplate(string modelName, string link)
        {
            return new EmailTemplate
            {
                Subject = $"Training completed for model: {modelName}",
                PlainText = $"Your model {modelName} has completed training.\n\n" +
                            $"You can access it here: {link}\n\n" +
                            $"Best regards,\nThe FotoGen Team",
                Html = $@"
<html>
<body>
    <h1>Training Completed</h1>
    <p>Your model <strong>{modelName}</strong> has completed training.</p>
    <p><a href='{link}'>Click here to access your model</a></p>
    <p>Best regards,<br/>The FotoGen Team</p>
</body>
</html>"
            };
        }

        public static EmailTemplate GetTrainingFailedTemplate(string modelName, string error)
        {
            return new EmailTemplate
            {
                Subject = $"Training failed for model: {modelName}",
                PlainText = $"We're sorry to inform you that training for model {modelName} has failed.\n\n" +
                            $"Error details: {error}\n\n" +
                            $"Please try again or contact support.\n\n" +
                            $"Best regards,\nThe FotoGen Team",
                Html = $@"
<html>
<body>
    <h1>Training Failed</h1>
    <p>We're sorry to inform you that training for model <strong>{modelName}</strong> has failed.</p>
    <p>Error details: <code>{error}</code></p>
    <p>Please try again or contact support.</p>
    <p>Best regards,<br/>The FotoGen Team</p>
</body>
</html>"
            };
        }
    }
}
