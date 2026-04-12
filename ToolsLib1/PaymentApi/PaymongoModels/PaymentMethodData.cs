using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymentMethodData
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
}