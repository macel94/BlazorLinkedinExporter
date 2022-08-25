using System.Text.Json.Serialization;

namespace LinkedinProxy.Dto.Authentication
{
    public class GetEmailResponse
    {
        [JsonPropertyName("elements")]
        public Element[]? Elements { get; set; }
    }

}
