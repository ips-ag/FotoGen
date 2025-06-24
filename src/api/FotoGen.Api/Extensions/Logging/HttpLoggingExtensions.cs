using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FotoGen.Extensions.Logging;

public static class HttpLoggingExtensions
{
    public static IServiceCollection ConfigureHttpLogging(this IServiceCollection services)
    {
        // exception handling
        services.AddSingleton<ExceptionStatusCodeMapper>();
        services.Configure<MvcOptions>(options => options.Filters.Add<GlobalExceptionFilter>());
        // request logging
        services.AddHttpLogging();
        services.AddSingleton<IConfigureOptions<HttpLoggingOptions>, ConfigureHttpLoggingOptions>();
        return services;
    }
}
