using Firebase.Auth;
using ToolsLib1.FirebaseTools;

namespace LogicLib1.Services.AppAuth;

public class AppAuthentication(IToolFirebaseAuth _authClient) : IAppAuthentication
{
    public async Task<UserCredential> LoginAsync(string email, string pass)
    {
        try
        {
            var resp = await _authClient.SignInAsync(email, pass);

            return resp;
        }
        catch (Exception ex)
        {
            throw new Exception($"Login failed: {ex.Message}");
        }
    }
}