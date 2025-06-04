using System.Reflection;
using FluentValidation;
using FotoGen.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FotoGen.Application;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddOptions<AppSettings>().BindConfiguration(AppSettings.SectionName);
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.Lifetime = ServiceLifetime.Scoped;
        });
        services.AddValidatorsFromAssembly(typeof(ApplicationDI).Assembly);
        return services;
    }
}
