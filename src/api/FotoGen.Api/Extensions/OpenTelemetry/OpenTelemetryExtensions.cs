using System.Reflection;
using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Resources;

namespace FotoGen.Extensions.OpenTelemetry;

internal static class OpenTelemetryExtensions
{
    private static readonly List<string> ConfigurationKeys =
    [
        "APPLICATIONINSIGHTS_CONNECTION_STRING",
        "ApplicationInsights:Connection:String",
        "AzureMonitor:ConnectionString"
    ];

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        if (ConfigurationKeys.All(key =>
            {
                var value = builder.Configuration.GetValue<string>(key);
                return !string.IsNullOrEmpty(value);
            }))
        {
            // no connection string provided, skip OpenTelemetry configuration
            return builder;
        }
        builder.Services.AddOpenTelemetry().UseAzureMonitor().ConfigureResource(resourceBuilder =>
        {
            var resourceAttributes = new Dictionary<string, object>
            {
                { "service.name", "api" },
                { "service.instance.id", Environment.MachineName },
                { "service.namespace", "FotoGen" },
                { "service.environment", builder.Environment.EnvironmentName }
            };
            var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString();
            if (version is not null)
            {
                resourceAttributes.Add("service.version", version);
            }
            resourceBuilder.AddAttributes(resourceAttributes);
        });
        return builder;
    }
}
