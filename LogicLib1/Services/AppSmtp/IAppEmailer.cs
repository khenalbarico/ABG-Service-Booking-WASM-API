
using CommonLib1.Models.Client;

namespace LogicLib1.Services.AppSmtp;

public interface IAppEmailer
{
    Task SendEmailAsync(ClientRequest req);
}
