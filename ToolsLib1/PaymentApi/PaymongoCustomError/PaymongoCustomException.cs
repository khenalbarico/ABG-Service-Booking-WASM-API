using System.Net;
using System.Text.Json;

namespace ToolsLib1.PaymentApi.PaymongoCustomError;

internal static class PaymongoCustomException1
{
    public static Exception CreatePaymongoException(this string respBody, HttpStatusCode statusCode)
    {
        try
        {
            var error = JsonSerializer.Deserialize<PaymongoErrorResp>(respBody);
            var first = error?.Errors.FirstOrDefault();

            if (first is not null)
            {
                return new InvalidOperationException(
                    $"PayMongo error ({(int)statusCode}): {first.Code} - {first.Detail}");
            }
        }
        catch
        {
        }

        return new InvalidOperationException($"PayMongo error ({(int)statusCode}): {respBody}");
    }
}