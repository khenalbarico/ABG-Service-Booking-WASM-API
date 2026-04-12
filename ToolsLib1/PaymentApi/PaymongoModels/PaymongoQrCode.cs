using System.Text.Json.Serialization;

namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymongoQrCode
{
    [JsonPropertyName("id")]
    public string? Id       { get; set; }

    [JsonPropertyName("amount")]
    public long Amount      { get; set; }

    [JsonPropertyName("image_url")]
    public string? ImageUrl { get; set; }

    [JsonPropertyName("label")]
    public string? Label    { get; set; }
}