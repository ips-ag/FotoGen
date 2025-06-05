using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.UseModel.Models;

public class UseModelResponseModel
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("input")]
    public InputModel Input { get; set; }

    [JsonPropertyName("logs")]
    public string Logs { get; set; }

    [JsonPropertyName("output")]
    public object Output { get; set; }

    [JsonPropertyName("data_removed")]
    public bool DataRemoved { get; set; }

    [JsonPropertyName("error")]
    public object Error { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("urls")]
    public UrlCollectionModel Urls { get; set; }
}
