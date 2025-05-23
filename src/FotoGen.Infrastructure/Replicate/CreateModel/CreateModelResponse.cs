using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.CreateModel
{
    public class CreateModelResponse
    {
        [JsonPropertyName("cover_image_url")]
        public string? CoverImageUrl { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("default_example")]
        public object? DefaultExample { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("github_url")]
        public string? GithubUrl { get; set; }

        [JsonPropertyName("latest_version")]
        public object? LatestVersion { get; set; }

        [JsonPropertyName("license_url")]
        public string? LicenseUrl { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("owner")]
        public string? Owner { get; set; }

        [JsonPropertyName("paper_url")]
        public string? PaperUrl { get; set; }

        [JsonPropertyName("run_count")]
        public int RunCount { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("visibility")]
        public string? Visibility { get; set; }

        [JsonPropertyName("weights_url")]
        public string? WeightsUrl { get; set; }
    }
}
