using MediatR;

namespace FotoGen.Application.Events;

public class ModelTrainingSucceededEvent : INotification
{
    public string UserEmail { get; }
    public string UserName { get; }
    public string ModelName { get; }

    public ModelTrainingSucceededEvent(string userEmail, string modelName, string userName)
    {
        UserEmail = userEmail;
        UserName = userName;
        ModelName = modelName;
    }
}
