namespace FotoGen.Infrastructure.Settings
{
    public class AzureStorageSettings
    {
        public const string Section = "AzureStorage";
        public string ConnectionString { get; set; }
        public string ContainerName {  get; set; }
    }
}
