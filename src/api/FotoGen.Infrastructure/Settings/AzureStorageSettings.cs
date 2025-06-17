namespace FotoGen.Infrastructure.Settings
{
    public class AzureStorageSettings
    {
        public const string SectionName = "AzureStorage";
        public required string ConnectionString { get; init; }
        public required string ContainerName {  get; init; }
        public required string TableName { get; init; }
    }
}
