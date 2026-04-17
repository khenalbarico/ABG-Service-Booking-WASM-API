using CommonLib1.Models.Client;

namespace LogicLib1.Services.AppPayment;

public interface IAppPaymentApi
{
    Task<string> ProcessClientPaymentAsync(string paymentIntentId, ClientRequest req);
}
