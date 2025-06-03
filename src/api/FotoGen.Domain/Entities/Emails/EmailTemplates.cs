namespace FotoGen.Domain.Entities.Emails
{
    public static class EmailTemplates
    {
        public static EmailTemplate GetTrainingCompletedTemplate(string modelName, string appLink, string sharedLink)
        {
            return new EmailTemplate
            {
                Subject = $"Your model '{modelName}' is ready to use",
                PlainText = $"Model training is completed.\n\n" +
                           $"You can use it by going to the app: {appLink}\n\n" +
                           $"Share it with others using this link: {sharedLink}\n\n" +
                           $"Best regards,\nThe FotoGen Team",
                Html = $@"
<html>
<body>
    <h1>Model Training Completed</h1>
    <p>Your model <strong>{modelName}</strong> is ready to use.</p>
    <p><a href='{appLink}'>Click here to access the app</a></p>
    <p>Share with others: <a href='{sharedLink}'>{sharedLink}</a></p>
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
