using System.Reflection;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Resources;

namespace FotoGen.Extensions.OpenTelemetry;

internal static class OpenTelemetryExtensions
{
    public static IServiceCollection ConfigureOpenTelemetry(
        this IServiceCollection services,
        IHostEnvironment environment)
    {
        services.AddOpenTelemetry().UseAzureMonitor().ConfigureResource(resourceBuilder =>
        {
            var resourceAttributes = new Dictionary<string, object>
            {
                { "service.name", "api" },
                { "service.instance.id", Environment.MachineName },
                { "service.namespace", "FotoGen" },
                { "service.environment", environment.EnvironmentName }
            };
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            if (version is not null)
            {
                resourceAttributes.Add("service.version", version);
            }
            resourceBuilder.AddAttributes(resourceAttributes);
        });
        return services;
    }
}
