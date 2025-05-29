using System.ComponentModel.DataAnnotations;

namespace FotoGen.Extensions.Security.Configuration;

internal class SecurityConfiguration
{
    public const string SectionName = "Security";

    [Required]
    public required AuthenticationConfiguration Authentication { get; set; }

    [Required]
    public required AuthorizationConfiguration Authorization { get; set; }
}
