using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymentIntentAttributes
{
    [JsonPropertyName("amount")]
    public long                Amount     { get; set; }

    [JsonPropertyName("status")]
    public string?             Status     { get; set; }

    [JsonPropertyName("client_key")]
    public string?             ClientKey  { get; set; }

    [JsonPropertyName("next_action")]
    public PaymongoNextAction? NextAction { get; set; }
}