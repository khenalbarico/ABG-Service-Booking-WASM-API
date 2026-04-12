using MailKit.Net.Smtp;
using MailKit.Security;

namespace ToolsLib1.EmailerTools;

public class MailkitSmtpClient(IMailkitSmtpCfg _cfg) : IToolEmailer
{
    public async Task SendAsync(
       string recepient,
       string subject = "",
       string body    = "")
    {
        var message = _cfg.CreateSmtpClient(recepient, subject, body);

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(
                _cfg.SmtpHost,
                _cfg.SmtpPort,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_cfg.UserName, _cfg.AppPassword);

            await smtp.SendAsync(message);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}