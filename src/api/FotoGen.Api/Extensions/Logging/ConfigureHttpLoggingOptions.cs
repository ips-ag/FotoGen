using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Options;

namespace FotoGen.Extensions.Logging;

internal class ConfigureHttpLoggingOptions : IConfigureOptions<HttpLoggingOptions>
{
    public void Configure(HttpLoggingOptions options)
    {
        options.CombineLogs = true;
        options.LoggingFields = HttpLoggingFields.RequestPropertiesAndHeaders |
            HttpLoggingFields.ResponsePropertiesAndHeaders |
            HttpLoggingFields.RequestBody |
            HttpLoggingFields.ResponseBody;
        options.RequestBodyLogLimit = 4 * 1024; // 4 KB
        options.ResponseBodyLogLimit = 4 * 1024; // 4 KB
    }
}
