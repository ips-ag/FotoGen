using System.Text.Json.Serialization;

namespace FotoGen.Requests;

public class GeneratePhotoRequest
{
    [JsonPropertyName("modelName")]
    public string? ModelName { get; set; }
    
    [JsonPropertyName("triggerWord")]
    public string? TriggerWord { get; set; }

    [JsonPropertyName("prompt")]
    [JsonRequired]
    public required string Prompt { get; set; }
}
