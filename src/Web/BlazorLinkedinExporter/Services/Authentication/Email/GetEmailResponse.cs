using System.Text.Json.Serialization;

namespace BlazorLinkedinExporter.Services.Authentication.Email
{
    public class GetEmailResponse
    {
        [JsonPropertyName("elements")]
        public Element[]? Elements { get; set; }
    }
}
