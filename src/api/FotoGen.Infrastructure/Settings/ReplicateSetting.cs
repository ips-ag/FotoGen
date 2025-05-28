namespace FotoGen.Infrastructure.Settings
{
    public class ReplicateSetting
    {
        public const string Section = "Replicate";
        public string BaseUrl { get; init; }
        public string Model { get; init; }
        public string Version { get; init; }
        public string Token { get; init; }
        public string Hardware { get; init; }
        public string Owner { get; init; }
        public string Visibility { get; init; }
        public int TimeoutSeconds { get; init; }
        public string Mode {  get; init; }
        public string OutputFormat { get; init; }
    }
}
