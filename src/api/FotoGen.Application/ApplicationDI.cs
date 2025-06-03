using System.Reflection;
using FluentValidation;
using FotoGen.Domain.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FotoGen.Application;

public static class ApplicationDI
{
    public static void AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(AppSettings.Section));
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.Lifetime = ServiceLifetime.Scoped;
        });
        services.AddValidatorsFromAssembly(typeof(ApplicationDI).Assembly);
    }
}
