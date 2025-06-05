namespace FotoGen.Infrastructure.Replicate.Configuration;

public class ReplicateSetting
{
    public const string SectionName = "Replicate";
    public required string BaseUrl { get; init; }
    public required TrainingSettings Training { get; init; }
    
    public required string Token { get; init; }
    public required string Owner { get; init; }
    public required int TimeoutSeconds { get; init; }
}

public class TrainingSettings
{
    public required string Model { get; init; }
    public required string Version { get; init; }
    public required string Hardware { get; init; }
    public required VisibilitySetting Visibility { get; init; }
}

public enum VisibilitySetting
{
    Public,
    Private
}
