using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MACEL94.github.io.Services.Authentication;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationState _anonymous;

    public AuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<PersistentAccessToken?>(TokenConstants.AccessToken);
        if (token == null)
            return _anonymous;
        if (string.IsNullOrWhiteSpace(token.AccessToken) || token.ValidUntil == null || token.ValidUntil < DateTimeOffset.UtcNow || string.IsNullOrEmpty(token.Email))
        {
            //Invalid tokens need to be removed
            await _localStorage.RemoveItemAsync(TokenConstants.AccessToken);
            return _anonymous;
        }
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, token.Email) }, "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string email, string firstName, string lastName)
    {
        //TODO REFACTOR, TAKE EVERYTHING ONLY FROM ACCESS TOKEN
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email), new Claim(ClaimTypes.Name, firstName), new Claim(ClaimTypes.Surname, lastName) }, "jwtAuthType"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
