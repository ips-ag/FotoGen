using System.Text.Json.Serialization;

namespace FotoGen.Infrastructure.Replicate.CreateModel
{
    public class CreateModelInput
    {
        [JsonPropertyName("owner")]
        public string Owner {  get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("visibility")]
        public string Visibility {  get; set; }
        [JsonPropertyName("hardware")]
        public string Hardware {  get; set; }
    }
}
