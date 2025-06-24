using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;

namespace FotoGen.Extensions.Logging;

public static class HttpLoggingExtensions
{
    public static IServiceCollection ConfigureHttpLogging(this IServiceCollection services)
    {
        services.AddHttpLogging();
        services.AddSingleton<IConfigureOptions<HttpLoggingOptions>, ConfigureHttpLoggingOptions>();
        return services;
    }
}
