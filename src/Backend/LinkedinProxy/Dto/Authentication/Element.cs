using System.Text.Json.Serialization;

namespace LinkedinProxy.Dto.Authentication
{
    public class Element
    {
        [JsonPropertyName("handle~")]
        public Handle? EmailHandle { get; set; }
        [JsonPropertyName("handle")]
        public string? HandleUrn { get; set; }
    }

}
