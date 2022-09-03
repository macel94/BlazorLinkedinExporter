using System.Text.Json.Serialization;

namespace BlazorLinkedinExporter.Services.Authentication.Email
{
    public class Element
    {
        [JsonPropertyName("handle~")]
        public Handle? EmailHandle { get; set; }
        [JsonPropertyName("handle")]
        public string? HandleUrn { get; set; }
    }
}
