namespace ToolsLib1.PaymentApi;

public interface IPaymongoCfg
{
    string SecretKey        { get; }
    string BaseUrl          { get; }
    string WebhookSecretKey { get; }
}