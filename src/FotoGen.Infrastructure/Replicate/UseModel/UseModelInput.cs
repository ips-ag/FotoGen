using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FotoGen.Infrastructure.Replicate.UseModel
{
    public class UseModelInput
    {
        [JsonPropertyName("version")]
        public string Version {  get; set; }
        [JsonPropertyName("input")]
        public Input Input { get; set; }
    }
    public class Input
    {
        [JsonPropertyName("model")]
        public string Model {  get; set; }
        [JsonPropertyName("prompt")]
        public string Prompt {  get; set; }
        [JsonPropertyName("output_format")]
        public string OutputFormat {  get; set; }
    }
}
