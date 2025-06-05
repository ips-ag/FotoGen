namespace FotoGen.Infrastructure.Settings;

public class EmailSettings
{
    public const string SectionName = "Email";
    public required string ConnectionString { get; init; }
    public required string SenderEmail { get; init; }
}
