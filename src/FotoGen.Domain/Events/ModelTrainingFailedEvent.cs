using MediatR;

namespace FotoGen.Domain.Events
{
    public class ModelTrainingFailedEvent : BaseModelTrainingEvent, INotification
    {
        public string Error { get; }
        public ModelTrainingFailedEvent(string id, string model, string version, string userEmail, string error)
        {
            Id = id;
            Model = model;
            Version = version;
            UserEmail = userEmail;
            Error = error;
        }
    }
}
