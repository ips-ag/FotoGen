using MediatR;

namespace FotoGen.Application.Events;

public class ModelTrainingFailedEvent : INotification
{
    public string UserEmail { get; }
    public string ModelName { get; }
    public string Error { get; }


    public ModelTrainingFailedEvent(string userEmail, string modelName, string error)
    {
        UserEmail = userEmail;
        ModelName = modelName;
        Error = error;
    }
}
