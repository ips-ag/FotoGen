using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.UseModel.Models;

public class UseModelRequestModel
{
    [JsonPropertyName("version")]
    [JsonRequired]
    public required string Version { get; init; }

    [JsonPropertyName("input")]
    [JsonRequired]
    public required InputModel Input { get; init; }
}
