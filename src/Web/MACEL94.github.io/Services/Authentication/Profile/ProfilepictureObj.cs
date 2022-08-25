using System.Text.Json.Serialization;

namespace MACEL94.github.io.Services.Authentication.Profile
{
    public class ProfilepictureObj
    {
        public string? DisplayImage { get; set; }
        [JsonPropertyName("displayImage~")]
        public Displayimage? DisplayImageObj { get; set; }
    }

}
