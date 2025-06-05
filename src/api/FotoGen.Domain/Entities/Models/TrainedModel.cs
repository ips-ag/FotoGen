namespace FotoGen.Domain.Entities.Models;

public record TrainedModel(string Owner, string Name, string? LatestVersion)
{
    public bool CanTrain => LatestVersion is not null;
}
