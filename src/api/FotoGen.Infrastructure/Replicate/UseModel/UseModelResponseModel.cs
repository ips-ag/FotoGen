
using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.UseModel
{
    public class UseModelResponseModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("input")]
        public InputData Input { get; set; }

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
        public Urls Urls { get; set; }
    }

    public class InputData
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("output_format")]
        public string OutputFormat { get; set; }

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }
    }

    public class Urls
    {
        [JsonPropertyName("cancel")]
        public string Cancel { get; set; }

        [JsonPropertyName("get")]
        public string Get { get; set; }

        [JsonPropertyName("stream")]
        public string Stream { get; set; }

        [JsonPropertyName("web")]
        public string Web { get; set; }
    }
}
