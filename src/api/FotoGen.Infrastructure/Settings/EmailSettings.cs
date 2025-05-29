namespace FotoGen.Infrastructure.Settings
{
    public class EmailSettings
    {
        public const string Section = "Email";
        public string ConnectionString {  get; init; }
        public string SenderEmail { get; init; }
    }
}
