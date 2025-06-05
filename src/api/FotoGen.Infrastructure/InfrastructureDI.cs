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
using FotoGen.Infrastructure.Replicate.Configuration;
using FotoGen.Infrastructure.Replicate.GetTrainedModelStatus.Converters;
using FotoGen.Infrastructure.Repositories;
using FotoGen.Infrastructure.Repositories.Requests;
using FotoGen.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FotoGen.Infrastructure;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IReplicateService, ReplicateService>();
        services.AddOptions<ReplicateSetting>()
            .BindConfiguration(ReplicateSetting.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<ModelTrainingSettings>()
            .BindConfiguration(ModelTrainingSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<EmailSettings>()
            .BindConfiguration(EmailSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddOptions<AzureStorageSettings>()
            .BindConfiguration(AzureStorageSettings.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddScoped<IModelTrainingRepository>(provider =>
        {
            var options = provider.GetRequiredService<IOptions<ModelTrainingSettings>>().Value;
            return new ModelTrainingRepository(options.CsvFilePath);
        });
        services.AddScoped<IEmailService, EmailService>();
        services.AddSingleton<TrainingStatusConverter>();
        services.AddHttpClient<IReplicateService, ReplicateService>((sp, client) =>
        {
            var settings = sp.GetRequiredService<IOptions<ReplicateSetting>>().Value;
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutSeconds);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.Token);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });
        services.AddHttpClient<IDownloadClient, DownloadClient>()
            .ConfigurePrimaryHttpMessageHandler(() =>
                new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                    AllowAutoRedirect = true
                });
        services.AddScoped(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<EmailSettings>>().Value;
            return new EmailClient(settings.ConnectionString);
        });
        services.AddScoped(provider =>
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

        return services;
    }
}
