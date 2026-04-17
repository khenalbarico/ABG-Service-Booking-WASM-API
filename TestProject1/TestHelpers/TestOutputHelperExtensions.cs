using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using ToolsLib1.FirebaseTools;
using LogicLib1.Services.AppDb;

namespace TestProject1.TestHelpers;

internal static class TestOutputHelperExtensions
{
    private static IHost? Host;
    public static T Get<T>(this ITestOutputHelper ctx) where T : class
    {
        Host ??= new HostBuilder()
            .ConfigureHostConfiguration(config =>
            {
                config.AddEnvironmentVariables();
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("testsettings.json", optional: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddAppCfg(context.Configuration);
                services.AddSingleton<IToolFirebaseDbOperations, FirebaseRealtimeDb1>();
                services.AddSingleton<IAppDbOperator, AppDbOperator>();
            })
            .Build();

        return Host.Services.GetRequiredService<T>();
    }
}
