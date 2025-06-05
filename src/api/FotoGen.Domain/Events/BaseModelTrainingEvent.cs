namespace FotoGen.Domain.Events;

public abstract class BaseModelTrainingEvent
{
    public string Id { get; set; }
    public string Model { get; set; }
    public string Version { get; set; }
    public string UserEmail { get; set; }
}
