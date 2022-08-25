using System.Text.Json.Serialization;

namespace LinkedinProxy.Dto.Authentication.Email
{
    public class GetEmailResponse
    {
        [JsonPropertyName("elements")]
        public Element[]? Elements { get; set; }
    }

}
