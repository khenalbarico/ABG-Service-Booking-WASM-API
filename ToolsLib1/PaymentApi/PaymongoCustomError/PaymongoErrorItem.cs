using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoCustomError;

internal sealed class PaymongoErrorItem
{
    [JsonPropertyName("code")]
    public string Code   { get; set; } = string.Empty;

    [JsonPropertyName("detail")]
    public string Detail { get; set; } = string.Empty;
}