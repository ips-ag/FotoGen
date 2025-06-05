using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.GetTrainedModel.Models;

public class VersionModel
{
    [JsonPropertyName("id")]
    [Required]
    public required string Id { get; init; }
}
