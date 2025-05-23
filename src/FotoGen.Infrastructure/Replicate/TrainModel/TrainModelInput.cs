using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.TrainModel
{
    public class TrainModelInput
    {
        [JsonPropertyName("input")]
        public ReplicateModelInput Input { get; set; } = default!;

        [JsonPropertyName("destination")]
        public string Destination { get; set; } = default!;
    }
    public class ReplicateModelInput
    {
        [JsonPropertyName("steps")]
        public int Steps { get; set; }

        [JsonPropertyName("lora_rank")]
        public int LoraRank { get; set; }

        [JsonPropertyName("optimizer")]
        public string Optimizer { get; set; } = default!;

        [JsonPropertyName("batch_size")]
        public int BatchSize { get; set; }

        [JsonPropertyName("resolution")]
        public string Resolution { get; set; } = default!;

        [JsonPropertyName("autocaption")]
        public bool AutoCaption { get; set; }

        [JsonPropertyName("input_images")]
        public string InputImages { get; set; } = default!;

        [JsonPropertyName("trigger_word")]
        public string TriggerWord { get; set; } = default!;

        [JsonPropertyName("learning_rate")]
        public double LearningRate { get; set; }

        [JsonPropertyName("wandb_project")]
        public string WandbProject { get; set; } = default!;

        [JsonPropertyName("wandb_save_interval")]
        public int WandbSaveInterval { get; set; }

        [JsonPropertyName("caption_dropout_rate")]
        public double CaptionDropoutRate { get; set; }

        [JsonPropertyName("cache_latents_to_disk")]
        public bool CacheLatentsToDisk { get; set; }

        [JsonPropertyName("wandb_sample_interval")]
        public int WandbSampleInterval { get; set; }

        [JsonPropertyName("gradient_checkpointing")]
        public bool GradientCheckpointing { get; set; }
    }
}
