using System.Net;
using System.Net.Http.Headers;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Interfaces;
using FotoGen.Infrastructure.BackgroundServices;
using FotoGen.Infrastructure.Email;
using FotoGen.Infrastructure.Replicate;
using FotoGen.Infrastructure.Repositories;
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
            services.AddScoped<IReplicateService, ReplicateService>();
            services.Configure<ReplicateSetting>(configuration.GetSection(ReplicateSetting.Section));
            services.Configure<ModelTrainingSettings>(configuration.GetSection(ModelTrainingSettings.Section));
            services.AddScoped<ITrainedModelRepository>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<ModelTrainingSettings>>().Value;
                return new TrainedModelRepository(options.CsvFilePath);
            });
            services.AddScoped<IEmailService, EmailService>();
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
            services.AddHostedService<ModelTrainingBackgroundService>();

        }
    }
}
