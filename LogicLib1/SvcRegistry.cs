using LogicLib1.Services.ApiRelayer;
using LogicLib1.Services.AppAuth;
using LogicLib1.Services.AppDb;
using LogicLib1.Services.AppSmtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToolsLib1.EmailerTools;
using ToolsLib1.FirebaseTools;
using ToolsLib1.PaymentApi;

namespace LogicLib1;

public static class SvcRegistry
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

    public static void AddSvcRegistry(
           this   IServiceCollection svc,
                  IConfiguration cfg)
    {
        svc.AddHttpClient();

        //AppCfg Registry
        svc.AddAppCfg(cfg);

        //App Logic Registry
        svc.AddSingleton<IAppDbOperator, AppDbOperator>();
        svc.AddSingleton<IAppEmailer, AppEmailer>();
        svc.AddSingleton<IAppAuthentication, AppAuthentication>();
        svc.AddSingleton<IApiRelay, ApiRelay>();
        svc.AddSingleton<IRelayDispatcher, RelayDispatcher>();

        //Tool Registry
        svc.AddSingleton<IToolPaymentApi, PaymongoQrph>();
        svc.AddSingleton<IToolFirebaseDbOperations, FirebaseRealtimeDb1>();
        svc.AddSingleton<IToolEmailer, MailkitSmtpClient>();
        svc.AddSingleton<IToolFirebaseAuth, FirebaseAuth>();
    }

}
