using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.CreateModel.Models;

public class CreateModelInputModel
{
    [JsonPropertyName("owner")]
    public required string Owner { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("visibility")]
    public string Visibility { get; set; }

    [JsonPropertyName("hardware")]
    public string? Hardware { get; set; }
}
