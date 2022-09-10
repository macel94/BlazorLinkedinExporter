using BlazorLinkedinExporter.Services.Authentication.AccessToken;

namespace BlazorLinkedinExporter.Services.Authentication
{
    public class PersistentAccessToken
    {
        public string? AccessToken { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        //Used during deserialization
        public PersistentAccessToken()
        {
            AccessToken = string.Empty;
            ValidUntil = null;
        }

        public PersistentAccessToken(AccessTokenResponse? obj)
        {
            if (obj == null) return;
            AccessToken = obj.AccessToken;
            ValidUntil = DateTimeOffset.Now.AddSeconds(obj.ExpiresIn ?? 0);
        }
    }
}
