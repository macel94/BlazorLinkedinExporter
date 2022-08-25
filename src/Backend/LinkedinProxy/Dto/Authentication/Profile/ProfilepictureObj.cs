using System.Text.Json.Serialization;

namespace LinkedinProxy.Dto.Authentication.Profile
{
    public class ProfilepictureObj
    {
        public string? DisplayImage { get; set; }
        [JsonPropertyName("displayImage~")]
        public Displayimage? DisplayImageObj { get; set; }
    }

}
