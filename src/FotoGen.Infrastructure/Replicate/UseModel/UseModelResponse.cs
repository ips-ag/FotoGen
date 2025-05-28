using Newtonsoft.Json;

namespace FotoGen.Infrastructure.Replicate.UseModel
{
    public class UseModelResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("input")]
        public InputData Input { get; set; }

        [JsonProperty("logs")]
        public string Logs { get; set; }

        [JsonProperty("output")]
        public object Output { get; set; }

        [JsonProperty("data_removed")]
        public bool DataRemoved { get; set; }

        [JsonProperty("error")]
        public object Error { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("urls")]
        public Urls Urls { get; set; }
    }

    public class InputData
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("output_format")]
        public string OutputFormat { get; set; }

        [JsonProperty("prompt")]
        public string Prompt { get; set; }
    }

    public class Urls
    {
        [JsonProperty("cancel")]
        public string Cancel { get; set; }

        [JsonProperty("get")]
        public string Get { get; set; }

        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("web")]
        public string Web { get; set; }
    }
}
