﻿using MACEL94.github.io.Services.Authentication.AccessToken;

namespace MACEL94.github.io.Services.Authentication
{
    public class PersistentAccessToken
    {
        public string? AccessToken { get; set; }
        public DateTimeOffset? ValidUntil { get; set; }
        public string? Email { get; set; }
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