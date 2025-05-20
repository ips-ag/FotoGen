using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace FotoGen.Application
{
    public static class ApplicationDI
    {
        public static void AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
