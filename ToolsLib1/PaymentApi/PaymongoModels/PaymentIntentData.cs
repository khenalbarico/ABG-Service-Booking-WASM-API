using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymentIntentData
{
    [JsonPropertyName("id")]
    public string?                 Id         { get; set; }

    [JsonPropertyName("attributes")]
    public PaymentIntentAttributes Attributes { get; set; } = new();
}