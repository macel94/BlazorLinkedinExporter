using System.Text.Json.Serialization;

namespace BlazorLinkedinExporter.Services.Authentication.Profile
{
    public class Data
    {
        [JsonPropertyName("com.linkedin.digitalmedia.mediaartifact.StillImage")]
        public Stillimage? StillImage { get; set; }
    }

}
