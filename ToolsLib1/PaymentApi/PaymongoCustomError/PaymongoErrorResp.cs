using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoCustomError;

internal sealed class PaymongoErrorResp
{
    [JsonPropertyName("errors")]
    public List<PaymongoErrorItem> Errors { get; set; } = [];
}