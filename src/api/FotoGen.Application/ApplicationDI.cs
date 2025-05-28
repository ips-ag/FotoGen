using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FotoGen.Application;

public static class ApplicationDI
{
    public static void AddApplicationDI(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(typeof(ApplicationDI).Assembly);
    }
}
