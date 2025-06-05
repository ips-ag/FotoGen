using FotoGen.Domain.Events;
using MediatR;

namespace FotoGen.Application.Events;

public class ModelTrainingSucceededEvent : BaseModelTrainingEvent, INotification
{
    public string UserName { get; set; }
    public string ModelName { get; set; }
    public ModelTrainingSucceededEvent(string id, string model, string version, string userEmail, string userName, string modelName)
    {
        Id = id;
        Model = model;
        Version = version;
        UserEmail = userEmail;
        UserName = userName;
        ModelName = modelName;
    }
}
