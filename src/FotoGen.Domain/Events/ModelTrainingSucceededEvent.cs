using MediatR;

namespace FotoGen.Domain.Events
{
    public class ModelTrainingSucceededEvent : BaseModelTrainingEvent, INotification
    {
        public ModelTrainingSucceededEvent(string id, string model, string version, string userEmail)
        {
            Id = id;
            Model = model;
            Version = version;
            UserEmail = userEmail;
        }
    }
}
