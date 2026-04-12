using System.Text.Json;
using ToolsLib1.PaymentApi.PaymongoCustomError;

namespace ToolsLib1.HttpClientTools;

internal static class HttpGetAsync
{
    public static async Task<T> GetAsync<T>(
           this HttpClient httpClient,
           string endPoint,
           object payload,
           CancellationToken ct = default)
    {
        using var resp = await httpClient.GetAsync(endPoint, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
            throw body.CreatePaymongoException(resp.StatusCode);

        var res = JsonSerializer.Deserialize<T>(body);

        return res is null ? throw new InvalidOperationException("Unable to deserialize PayMongo response.") : res;
    }
}