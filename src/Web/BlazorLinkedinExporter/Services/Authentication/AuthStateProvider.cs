using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorLinkedinExporter.Services.Authentication;

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
        if (string.IsNullOrWhiteSpace(token.AccessToken) || token.ValidUntil == null || token.ValidUntil < DateTimeOffset.UtcNow)
        {
            await _localStorage.RemoveItemAsync(TokenConstants.AccessToken);
            return _anonymous;
        }
        //TODO Request here email and username if not already in the persisted data

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token.AccessToken);

        var claims = new List<Claim>();

        if (token.Email is not null)
        {
            claims.Add(new Claim(ClaimTypes.Email, token.Email));
        }

        if (token.FirstName is not null)
        {
            claims.Add(new Claim(ClaimTypes.Name, token.FirstName));
        }

        if (token.LastName is not null)
        {
            claims.Add(new Claim(ClaimTypes.Surname, token.LastName));
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "jwtAuthType")));
    }

    public void NotifyUserAuthentication(string email, string firstName, string lastName)
    {
        ClaimsPrincipal authenticatedUser = new (new ClaimsIdentity(new Claim[] { new (ClaimTypes.Email, email), new (ClaimTypes.Name, firstName), new (ClaimTypes.Surname, lastName) }, "jwtAuthType"));
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
