using LogicLib1;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToolsLib1.EmailerTools;
using ToolsLib1.FirebaseTools;
using ToolsLib1.PaymentApi;

namespace TestProject1.TestHelpers;

internal static class AppCfgRegistry1
{
    public static void AddAppCfg(
       this IServiceCollection svc,
            IConfiguration cfg)
    {
        var appCfg = new AppCfg();

        cfg.GetSection("Firebase").Bind(appCfg);
        cfg.GetSection("MailkitSmtp").Bind(appCfg);
        cfg.GetSection("Paymongo").Bind(appCfg);

        svc.AddSingleton<IFirebaseCfg>(appCfg);
        svc.AddSingleton<IMailkitSmtpCfg>(appCfg);
        svc.AddSingleton<IPaymongoCfg>(appCfg);
    }
}
