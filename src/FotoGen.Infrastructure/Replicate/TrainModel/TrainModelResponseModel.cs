using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.TrainModel
{
    public class TrainModelResponseModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("model")]
        public string Model { get; set; } = default!;

        [JsonPropertyName("version")]
        public string Version { get; set; } = default!;

        [JsonPropertyName("input")]
        public ReplicateModelInput Input { get; set; } = default!;

        [JsonPropertyName("logs")]
        public string? Logs { get; set; }

        [JsonPropertyName("output")]
        public object? Output { get; set; }

        [JsonPropertyName("data_removed")]
        public bool DataRemoved { get; set; }

        [JsonPropertyName("error")]
        public object? Error { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("urls")]
        public ReplicateModelUrls Urls { get; set; } = default!;
    }
    public class ReplicateModelUrls
    {
        [JsonPropertyName("cancel")]
        public string Cancel { get; set; } = default!;

        [JsonPropertyName("get")]
        public string Get { get; set; } = default!;

        [JsonPropertyName("stream")]
        public string Stream { get; set; } = default!;

        [JsonPropertyName("web")]
        public string Web { get; set; } = default!;
    }
}
