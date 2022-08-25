using System.Text.Json.Serialization;

namespace MACEL94.github.io.Services.Authentication
{
    public class Element
    {
        [JsonPropertyName("handle~")]
        public Handle? EmailHandle { get; set; }
        [JsonPropertyName("handle")]
        public string? HandleUrn { get; set; }
    }
}
