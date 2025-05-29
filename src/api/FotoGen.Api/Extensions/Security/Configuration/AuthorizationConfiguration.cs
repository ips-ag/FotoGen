using System.ComponentModel.DataAnnotations;

namespace FotoGen.Extensions.Security.Configuration;

internal class AuthorizationConfiguration
{
    [Required]
    public required string PolicyName { get; set; }

    [Required]
    public required string[] RequiredClaims { get; set; }
}
