using FotoGen.Extensions.Security.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FotoGen.Extensions.Security;

public static class SecurityExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services)
    {
        services.AddOptions<SecurityConfiguration>()
            .BindConfiguration(SecurityConfiguration.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddTransient<IConfigureOptions<AuthenticationOptions>, ConfigureAuthenticationOptions>();
        services.AddTransient<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
        services.AddAuthentication().AddJwtBearer();
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<AuthorizationOptions>, ConfigureAuthorizationOptions>();
        services.AddAuthorization();
    }
}
