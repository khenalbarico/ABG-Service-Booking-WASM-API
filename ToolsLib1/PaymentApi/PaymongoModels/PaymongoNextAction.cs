using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymongoNextAction
{
    [JsonPropertyName("type")]
    public string?         Type { get; set; }

    [JsonPropertyName("code")]
    public PaymongoQrCode? Code { get; set; }
}