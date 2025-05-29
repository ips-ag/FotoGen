namespace FotoGen.Domain.Entities.Emails
{
    public record EmailTemplate
    {
        public string Subject { get; set; }
        public string PlainText { get; set; }
        public string Html { get; set; }
    }
}
