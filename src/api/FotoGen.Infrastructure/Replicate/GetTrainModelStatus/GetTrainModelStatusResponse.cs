using System.Text.Json.Serialization;
using FotoGen.Infrastructure.Replicate.TrainModel;

namespace FotoGen.Infrastructure.Replicate.GetTrainModelStatus
{
    public class GetTrainModelStatusResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = default!;

        [JsonPropertyName("model")]
        public string Model { get; set; } = default!;

        [JsonPropertyName("version")]
        public string Version { get; set; } = default!;

        [JsonPropertyName("input")]
        public ReplicateModelInput Input { get; set; } = default!;

        [JsonPropertyName("output")]
        public ReplicateTrainingOutput? Output { get; set; }

        [JsonPropertyName("data_removed")]
        public bool DataRemoved { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = default!;

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("started_at")]
        public DateTime? StartedAt { get; set; }

        [JsonPropertyName("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [JsonPropertyName("urls")]
        public ReplicateTrainingUrls Urls { get; set; } = default!;

        [JsonPropertyName("metrics")]
        public ReplicateTrainingMetrics? Metrics { get; set; }
    }

    public class ReplicateTrainingOutput
    {
        [JsonPropertyName("version")]
        public string Version { get; set; } = default!;

        [JsonPropertyName("weights")]
        public string Weights { get; set; } = default!;
    }

    public class ReplicateTrainingUrls
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

    public class ReplicateTrainingMetrics
    {
        [JsonPropertyName("predict_time")]
        public double PredictTime { get; set; }
    }
}
