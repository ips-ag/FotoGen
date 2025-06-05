using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.UseModel.Models;

public class UrlCollectionModel
{
    [JsonPropertyName("cancel")]
    public string? Cancel { get; init; }

    [JsonPropertyName("get")]
    public string? Get { get; init; }

    [JsonPropertyName("stream")]
    [JsonRequired]
    public required string Stream { get; init; }

    [JsonPropertyName("web")]
    public string? Web { get; init; }
}
