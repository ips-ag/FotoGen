namespace FotoGen.Infrastructure.Settings
{
    public class RateLimitSettings
    {
        public const string SectionName = "RateLimiting";

        public required RateLimitPolicy PhotoGeneration { get; init; }
        public required RateLimitPolicy ModelTraining { get; init; }
    }

    public class RateLimitPolicy
    {
        public required int PermitLimit { get; init; }
        public required int WindowInDays { get; init; }
    }
}
