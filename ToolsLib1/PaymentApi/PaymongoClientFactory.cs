using System.Net.Http.Headers;
using System.Text;

namespace ToolsLib1.PaymentApi;

internal static class PaymongoClientFactory
{
    public static HttpClient CreatePaymongoClient(this IPaymongoCfg cfg)
    {
        var basicToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(cfg.SecretKey));

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(cfg.BaseUrl),
        };

        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", basicToken);

        httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        return httpClient;
    }
}