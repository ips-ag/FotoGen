using FotoGen.Extensions.Security.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace FotoGen.Extensions.Security;

internal class ConfigureAuthorizationOptions : IConfigureOptions<AuthorizationOptions>
{
    private readonly IOptionsMonitor<SecurityConfiguration> _options;

    public ConfigureAuthorizationOptions(IOptionsMonitor<SecurityConfiguration> options)
    {
        _options = options;
    }

    public void Configure(AuthorizationOptions options)
    {
        var settings = _options.CurrentValue.Authorization;
        options.AddPolicy(
            settings.PolicyName,
            policy =>
            {
                foreach (string claim in settings.RequiredClaims)
                {
                    policy.RequireClaim(claim);
                }
            });
    }
}