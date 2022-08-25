using System.Text.Json.Serialization;

namespace LinkedinProxy.Dto.Authentication.Email
{
    public class Handle
    {
        [JsonPropertyName("emailAddress")]
        public string? Email { get; set; }
    }

}
