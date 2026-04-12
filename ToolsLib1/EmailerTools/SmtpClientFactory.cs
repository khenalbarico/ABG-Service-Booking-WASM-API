using MimeKit;

namespace ToolsLib1.EmailerTools;

public static class SmtpClientFactory1
{
    public static MimeMessage CreateSmtpClient(
           this   IMailkitSmtpCfg cfg,
           string                 recipient,
           string                 subject,
           string                 body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(cfg.SenderName, cfg.SenderEmail));
        message.To.Add(MailboxAddress.Parse(recipient));
        message.Subject = subject;

        message.Body = new TextPart("html")
        {
            Text = body
        };

        return message;
    }
}