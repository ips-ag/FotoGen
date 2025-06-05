using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.GetTrainedModel.Models;

public class GetTrainedModelResponseModel
{
    [JsonPropertyName("owner")]
    [Required]
    public required string Owner { get; init; }

    [JsonPropertyName("name")]
    [Required]
    public required string Name { get; init; }

    [JsonPropertyName("latest_version")]
    public VersionModel? LatestVersion { get; init; }
}
