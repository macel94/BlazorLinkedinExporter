using System.Text.Json.Serialization;

namespace MACEL94.github.io.Services.Authentication.Profile
{
    public class Data
    {
        [JsonPropertyName("com.linkedin.digitalmedia.mediaartifact.StillImage")]
        public Stillimage? StillImage { get; set; }
    }

}
