namespace FotoGen.Domain.Settings
{
    public class UserUsageLimitationInDaySettings
    {
        public const string SectionName = "UserUsageLimitationInDay";
        public required int Training { get; init; }
        public required int PhotoGeneration { get; init; }
    }
}
