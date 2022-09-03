using System.Text.Json.Serialization;

namespace BlazorLinkedinExporter.Services.Authentication.AccessToken
{
    public class AccessTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int? ExpiresIn { get; set; }
    }
}
