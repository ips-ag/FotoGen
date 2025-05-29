namespace FotoGen.Extensions.OpenApi.Configuration;

internal class SwaggerAuthenticationConfiguration
{
    public Uri? AuthorizationUrl { get; set; }

    public Uri? TokenUrl { get; set; }

    public ScopeConfiguration[]? Scopes { get; set; }
    public string? ClientId { get; set; }
}
