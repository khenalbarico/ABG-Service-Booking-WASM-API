using Firebase.Auth;
using Firebase.Auth.Providers;
using Firebase.Database;

namespace ToolsLib1.FirebaseTools;

internal static class FirebaseClientFactory
{
    public static FirebaseAuthClient CreateAuthClient(this IFirebaseCfg cfg)
    {
        return new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey     = cfg.ApiKey,
            AuthDomain = cfg.AuthDomain,
            Providers  =
            [
                new EmailProvider()
            ]
        });
    }

    public static FirebaseClient CreateDbClient(this IFirebaseCfg cfg)
    {
        return new FirebaseClient(cfg.DatabaseUrl);
    }
}