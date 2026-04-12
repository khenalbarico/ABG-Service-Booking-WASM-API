namespace ToolsLib1.PaymentApi.PaymongoModels;

public sealed class PaymongoQrphChargeResult
{
    public string  PaymentIntentId     { get; set; } = string.Empty;
    public string  PaymentIntentStatus { get; set; } = string.Empty;
    public string  PaymentMethodId     { get; set; } = string.Empty;
    public long    AmountCentavos      { get; set; }
    public decimal AmountPhp           { get; set; }
    public string  QrCodeId            { get; set; } = string.Empty;
    public string  QrImageUrl          { get; set; } = string.Empty;
    public string  QrLabel             { get; set; } = string.Empty;
    public string  NextActionType      { get; set; } = string.Empty;
    public string  RawResponse         { get; set; } = string.Empty;
}