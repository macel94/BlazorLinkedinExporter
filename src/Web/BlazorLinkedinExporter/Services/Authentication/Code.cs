using Blazored.LocalStorage;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BlazorLinkedinExporter.Services.Authentication;

public class Code
{
    public const string ClientId = "78t8sa3mzru5lu";

    public static string State = "";

    public static string CodeVerifier = "";

    public static string CodeChallenge = "";

    public static async Task InitAsync(ILocalStorageService localStorageService)
    {
        var seed = int.Parse(DateTimeOffset.UtcNow.Ticks.ToString()[^9..]);
        var random = new Random(seed);

        CodeVerifier = GenerateNonce(random);
        CodeChallenge = GenerateCodeChallenge(CodeVerifier);
        State = GenerateRandomState(random);

        await localStorageService.SetItemAsync(nameof(CodeChallenge), CodeChallenge);
        await localStorageService.SetItemAsync(nameof(CodeVerifier), CodeVerifier);
        await localStorageService.SetItemAsync(nameof(State), State);
    }

    private static string GenerateRandomState(Random random)
    {
        return random.Next(111111111, 999999999).ToString();
    }

    private static string GenerateNonce(Random random)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz123456789";
        var nonce = new char[128];
        for (int i = 0; i < nonce.Length; i++)
        {
            nonce[i] = chars[random.Next(chars.Length)];
        }

        return new string(nonce);
    }

    private static string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        var b64Hash = Convert.ToBase64String(hash);
        var code = Regex.Replace(b64Hash, "\\+", "-");
        code = Regex.Replace(code, "\\/", "_");
        code = Regex.Replace(code, "=+$", "");
        return code;
    }
}
