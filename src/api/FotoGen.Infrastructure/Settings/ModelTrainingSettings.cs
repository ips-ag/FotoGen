namespace FotoGen.Infrastructure.Settings;

public class ModelTrainingSettings
{
    public const string SectionName = "ModelTraining";
    public required string CsvFilePath { get; init; }
}
