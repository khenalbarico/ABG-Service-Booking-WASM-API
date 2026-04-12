using CommonLib1.Models.Authentication;

namespace LogicLib1.Services.AppAuth;

public interface IAppAuthentication
{
    Task<AuthResp> LoginAsync(string email, string password);
}