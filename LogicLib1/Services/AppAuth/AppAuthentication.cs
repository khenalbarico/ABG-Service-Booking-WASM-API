using CommonLib1.Models.Authentication;
using ToolsLib1.FirebaseTools;

namespace LogicLib1.Services.AppAuth;

public class AppAuthentication(IToolFirebaseAuth _authClient) : IAppAuthentication
{
    public async Task<AuthResp> LoginAsync(string email, string password)
    {
        try
        {
            var resp = await _authClient.SignInAsync(email, password);

            return resp;
        }
        catch (Exception ex)
        {
            throw new Exception($"Login failed: {ex.Message}");
        }
    }
}