using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.UseModel.Models;

public class InputModel
{
    [JsonPropertyName("model")]
    [JsonRequired]
    public string Model { get; init; } = "dev";

    [JsonPropertyName("prompt")]
    [JsonRequired]
    public required string Prompt { get; init; }

    [JsonPropertyName("output_format")]
    [JsonRequired]
    public string OutputFormat { get; init; } = "png";
}
