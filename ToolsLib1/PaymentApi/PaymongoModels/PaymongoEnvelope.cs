using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymongoEnvelope<T>
{
    [JsonPropertyName("data")]
    public T Data { get; set; } = default!;
}