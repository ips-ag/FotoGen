namespace FotoGen.Extensions.OpenApi.Configuration;

internal class SwaggerConfiguration
{
    public const string SectionName = "Swagger";

    public SwaggerAuthenticationConfiguration? Authentication { get; set; }
}
