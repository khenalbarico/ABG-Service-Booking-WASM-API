using ToolsLib1.EmailerTools;
using ToolsLib1.FirebaseTools;
using ToolsLib1.PaymentApi;

namespace LogicLib1;

public class AppCfg :
             IFirebaseCfg,
             IMailkitSmtpCfg,
             IPaymongoCfg
{
    public string ApiKey           { get; set; } = "";
    public string AuthDomain       { get; set; } = "";
    public string DatabaseUrl      { get; set; } = "";
    public string SenderEmail      { get; set; } = "";
    public string SenderName       { get; set; } = "";
    public string AppPassword      { get; set; } = "";
    public string UserName         { get; set; } = "";
    public string SmtpHost         { get; set; } = "";
    public int    SmtpPort         { get; set; }
    public string SecretKey        { get; set; } = "";
    public string BaseUrl          { get; set; } = "";
    public string WebhookSecretKey { get; set; } = "";
}