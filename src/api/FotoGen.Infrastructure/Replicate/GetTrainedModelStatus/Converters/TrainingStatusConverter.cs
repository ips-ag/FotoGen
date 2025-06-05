using FotoGen.Domain.Entities.Models;

namespace FotoGen.Infrastructure.Replicate.GetTrainedModelStatus.Converters;

public class TrainingStatusConverter
{
    public ModelTrainingStatus? ToDomain(string? model)
    {
        return model?.ToLowerInvariant() switch
        {
            "starting" => ModelTrainingStatus.InProgress,
            "processing" => ModelTrainingStatus.InProgress,
            "succeeded" => ModelTrainingStatus.Succeeded,
            "canceled" => ModelTrainingStatus.Canceled,
            "failed" => ModelTrainingStatus.Failed,
            _ => null
        };
    }
}
