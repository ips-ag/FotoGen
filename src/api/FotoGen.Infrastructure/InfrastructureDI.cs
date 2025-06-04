using System.Net;
using System.Net.Http.Headers;
using Azure.Communication.Email;
using Azure.Storage.Blobs;
using FotoGen.Application.Interfaces;
using FotoGen.Domain.Repositories;
using FotoGen.Infrastructure.AzureStorage;
using FotoGen.Infrastructure.BackgroundServices;
using FotoGen.Infrastructure.Email;
using FotoGen.Infrastructure.Replicate;
using FotoGen.Infrastructure.Repositories;
using FotoGen.Infrastructure.Repositories.Requests;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure;

public static class InfrastructureDI
{
    public static void AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IReplicateService, ReplicateService>();
        services.Configure<ReplicateSetting>(configuration.GetSection(ReplicateSetting.Section));
        services.Configure<ModelTrainingSettings>(configuration.GetSection(ModelTrainingSettings.Section));
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.Section));
        services.Configure<AzureStorageSettings>(configuration.GetSection(AzureStorageSettings.Section));
        services.AddScoped<IModelTrainingRepository>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<ModelTrainingSettings>>().Value;
            return new ModelTrainingRepository(options.CsvFilePath);
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
        services.AddHttpClient<IDownloadClient, DownloadClient>().ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = true
            });
        services.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<EmailSettings>>().Value;
            return new EmailClient(settings.ConnectionString);
        });
        services.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<AzureStorageSettings>>().Value;
            return new BlobServiceClient(settings.ConnectionString);
        });
        services.AddScoped<IAzureStorageService, AzureStorageService>();
        services.AddHostedService<ModelTrainingBackgroundService>();
        
        // request context
        services.AddSingleton<RequestContextAccessor>();
        services.AddSingleton<RequestContextFactory>();
        services.AddScoped<IRequestContextRepository, RequestContextRepository>();
    }
}
