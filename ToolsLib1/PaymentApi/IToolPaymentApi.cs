using CommonLib1.Models.Client;
using ToolsLib1.PaymentApi.PaymongoModels;

namespace ToolsLib1.PaymentApi;

public interface IToolPaymentApi
{
    Task<PaymongoQrphChargeResult> CreateQrphChargeAsync(
         ClientRequest     req,
         CancellationToken ct = default);
    Task<string> GetPaymentIntentStatusAsync(
         string            paymentIntentId,
         CancellationToken ct = default);
}