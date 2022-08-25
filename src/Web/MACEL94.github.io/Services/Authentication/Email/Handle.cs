using System.Text.Json.Serialization;

namespace MACEL94.github.io.Services.Authentication.Email
{
    public class Handle
    {
        [JsonPropertyName("emailAddress")]
        public string? Email { get; set; }
    }
}
