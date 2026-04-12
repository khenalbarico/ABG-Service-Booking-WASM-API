using Firebase.Auth;
using System.Text;
using System.Text.Json;

namespace ToolsLib1.FirebaseTools;

public class FirebaseAuth(IFirebaseCfg _cfg, HttpClient _httpClient) : IToolFirebaseAuth
{
    readonly FirebaseAuthClient _authClient = _cfg.CreateAuthClient();

    public async Task<UserCredential> SignInAsync(
        string email,
        string password)
    {
        var res = await _authClient.SignInWithEmailAndPasswordAsync(email, password);

        //if (!res.User.Info.IsEmailVerified)
        //{
        //    _authClient.SignOut();
        //    throw new Exception("Email's not yet verified. Please verify the email first.");
        //}

        return res;
    }

    public async Task SignUpAsync(
        string email,
        string password)
    {
        var res = await _authClient.CreateUserWithEmailAndPasswordAsync(email, password);

        var token = await res.User.GetIdTokenAsync();

        await SendEmailVerificationAsync(token);

        SignOut();
    }

    async Task SendEmailVerificationAsync(string idToken)
    {
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key={_cfg.ApiKey}";

        var requestBody = new
        {
            requestType = "VERIFY_EMAIL",
            idToken
        };

        var json = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var res = await _httpClient.PostAsync(url, content);

        var resTxt = await res.Content.ReadAsStringAsync();

        if (!res.IsSuccessStatusCode)
            throw new InvalidOperationException($"Failed to send verification email: {resTxt}");
    }

    public void SignOut()
        => _authClient.SignOut();
}