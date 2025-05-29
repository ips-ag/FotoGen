using Asp.Versioning;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FotoGen.Externsions.OpenApi;

public static class OpenApiExtensions
{
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
        // services.AddEndpointsApiExplorer();
        services.AddApiVersioning(o =>
        {
            o.ApiVersionReader = new UrlSegmentApiVersionReader();
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
        }).AddApiExplorer(o =>
        {
            o.SubstituteApiVersionInUrl = true;
        });
    }

    public static void AddSwaggerGenRespectingCustomApiVersioning(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();
        services.AddSwaggerGenNewtonsoftSupport();
    }
}
