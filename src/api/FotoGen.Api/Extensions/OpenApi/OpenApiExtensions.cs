using Asp.Versioning;
using FotoGen.Extensions.OpenApi.Configuration;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FotoGen.Extensions.OpenApi;

public static class OpenApiExtensions
{
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
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
        services.AddOptions<SwaggerConfiguration>()
            .BindConfiguration(SwaggerConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureGenSwaggerOptions>();
        services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());
        services.AddSwaggerGenNewtonsoftSupport();
    }
}
