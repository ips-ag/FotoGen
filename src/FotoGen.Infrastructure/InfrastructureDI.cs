using System.Net;
using System.Net.Http.Headers;
using FotoGen.Application.Interfaces;
using FotoGen.Infrastructure.Replicate;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure
{
    public static class InfrastructureDI
    {
        public static void AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ReplicateSetting>(configuration.GetSection(ReplicateSetting.Section));
            services.AddHttpClient<IReplicateService, ReplicateService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<ReplicateSetting>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
            services.AddHttpClient<IDownloadClient, DownloadClient>().ConfigurePrimaryHttpMessageHandler(
                () => new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    AllowAutoRedirect = true
                });
        }
    }
}
