namespace FotoGen.Domain.Entities.Emails;

public static class EmailTemplates
{
    public static EmailTemplate GetTrainingCompletedTemplate(string modelName, string appUrl, string shareUrl)
    {
        return new EmailTemplate(
            Subject: $"Your model '{modelName}' is ready to use",
            PlainText: $"""
                         Model training is completed.

                         You can use it by going to the app: {appUrl}

                         Share it with others using this link: {shareUrl}

                         Best regards,
                         The FotoGen Team
                        """,
            Html: $"""
                   <html>
                       <body>
                           <h1>Model Training Completed</h1>
                           <p>Your model <strong>{modelName}</strong> is ready to use.</p>
                           <p><a href='{appUrl}'>Click here to access the app</a></p>
                           <p>Share with others: <a href='{shareUrl}'>{shareUrl}</a></p>
                           <p>Best regards,<br/>The FotoGen Team</p>
                       </body>
                   </html>
                   """
        );
    }

    public static EmailTemplate GetTrainingFailedTemplate(string modelName, string error)
    {
        return new EmailTemplate(
            Subject: $"Training failed for model: {modelName}",
            PlainText: $"""
                        We're sorry to inform you that training for model {modelName} has failed.

                        Error details: {error}

                        Please try again or contact support.

                        Best regards,
                        The FotoGen Team
                        """,
            Html: $"""
                   <html>
                   <body>
                       <h1>Training Failed</h1>
                       <p>We're sorry to inform you that training for model <strong>{modelName}</strong> has failed.</p>
                       <p>Error details: <code>{error}</code></p>
                       <p>Please try again or contact support.</p>
                       <p>Best regards,<br/>The FotoGen Team</p>
                   </body>
                   </html>
                   """
        );
    }
}
