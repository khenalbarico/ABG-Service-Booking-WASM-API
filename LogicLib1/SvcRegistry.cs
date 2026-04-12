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
           this IServiceCollection svc,
                  IConfiguration cfg)
    {
        svc.AddHttpClient();

        svc.AddAppCfg(cfg);
        svc.AddRelayServices();

        svc.AddSingleton<IApiRelay, ApiRelay>();
        svc.AddSingleton<IRelayDispatcher, RelayDispatcher>();
    }

    public static void AddRelayServices(
           this IServiceCollection svc)
    {
        var relayRegistry = new RelayServiceRegistry();
        svc.AddSingleton(relayRegistry);

        svc.AddRelaySingleton<IAppDbOperator, AppDbOperator>(relayRegistry);
        svc.AddRelaySingleton<IAppEmailer, AppEmailer>(relayRegistry);
        svc.AddRelaySingleton<IAppAuthentication, AppAuthentication>(relayRegistry);
        svc.AddRelaySingleton<IToolPaymentApi, PaymongoQrph>(relayRegistry);
        svc.AddRelaySingleton<IToolFirebaseDbOperations, FirebaseRealtimeDb1>(relayRegistry);
        svc.AddRelaySingleton<IToolEmailer, MailkitSmtpClient>(relayRegistry);
        svc.AddRelaySingleton<IToolFirebaseAuth, FirebaseAuth>(relayRegistry);
    }
}