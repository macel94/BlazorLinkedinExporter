using System.Text.Json.Serialization;

namespace LinkedinProxy.Dto.Authentication.Profile
{
    public class Data
    {
        [JsonPropertyName("com.linkedin.digitalmedia.mediaartifact.StillImage")]
        public Stillimage? StillImage { get; set; }
    }

}
