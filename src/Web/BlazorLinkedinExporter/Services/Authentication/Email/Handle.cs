using System.Text.Json.Serialization;

namespace BlazorLinkedinExporter.Services.Authentication.Email
{
    public class Handle
    {
        [JsonPropertyName("emailAddress")]
        public string? Email { get; set; }
    }
}
