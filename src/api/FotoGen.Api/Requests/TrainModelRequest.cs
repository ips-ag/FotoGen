using System.Text.Json.Serialization;

namespace FotoGen.Requests;

public class TrainModelRequest
{
    [JsonPropertyName("imageUrl")]
    [JsonRequired]
    public required string ImageUrl { get; set; }
}
