namespace ToolsLib1.EmailerTools;

public interface IMailkitSmtpCfg
{
    string SenderEmail { get; set; }
    string SenderName  { get; set; }
    string AppPassword { get; set; }
    string UserName    { get; set; }
    string SmtpHost    { get; set; }
    int    SmtpPort    { get; set; }
}