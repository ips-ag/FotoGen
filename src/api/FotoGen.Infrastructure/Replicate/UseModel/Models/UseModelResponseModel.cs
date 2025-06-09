using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.UseModel.Models;

public class UseModelResponseModel
{
    [JsonPropertyName("input")]
    [JsonRequired]
    public required InputModel Input { get; set; }

    [JsonPropertyName("urls")]
    [JsonRequired]
    public required UrlCollectionModel Urls { get; set; }
}
