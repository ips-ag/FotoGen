namespace FotoGen.Domain.Entities.Models;

public record ModelTraining(
    string Id,
    string ModelName,
    string UserEmail,
    string UserName,
    string ImageUrl,
    string TriggerWord,
    ModelTrainingStatus TrainingStatus,
    DateTime CreatedAt,
    DateTime? SucceededAt = null,
    string? CanceledUrl = null
);
