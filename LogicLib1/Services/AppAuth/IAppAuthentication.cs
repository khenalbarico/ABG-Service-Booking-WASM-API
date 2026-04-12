using Firebase.Auth;

namespace LogicLib1.Services.AppAuth;

public interface IAppAuthentication
{
    Task<UserCredential> LoginAsync(string email, string pass);
}