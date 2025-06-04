namespace FotoGen.Infrastructure.Settings;

public class ReplicateSetting
{
    public const string SectionName = "Replicate";
    public required string BaseUrl { get; init; }
    public required string Model { get; init; }
    public required string Version { get; init; }
    public required string Token { get; init; }
    public required string Hardware { get; init; }
    public required string Owner { get; init; }
    public required string Visibility { get; init; }
    public required int TimeoutSeconds { get; init; }
    public required string Mode { get; init; }
    public required string OutputFormat { get; init; }
}
