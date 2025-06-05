namespace FotoGen.Application.Configuration;

public class AppSettings
{
    public const string SectionName = "App";
    public required string Host { get; init; }
}
