using System.Text;
using System.Text.Json;
using ToolsLib1.PaymentApi.PaymongoCustomError;

namespace ToolsLib1.HttpClientTools;

internal static class HttpPostAsync
{
    public static async Task<T> PostAsync<T>(
           this HttpClient   httpClient,
           string            endPoint,
           object            payload,
           CancellationToken ct = default)
    {
              var json    = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        using var resp    = await httpClient.PostAsync(endPoint, content, ct);
              var body    = await resp.Content.ReadAsStringAsync(ct);

        if (!resp.IsSuccessStatusCode)
            throw body.CreatePaymongoException(resp.StatusCode);

        var res = JsonSerializer.Deserialize<T>(body);

        return res is null ? throw new InvalidOperationException("Unable to deserialize PayMongo response.") : res;
    }
}