using CommonLib1.Models.Client;
using LogicLib1.Services.AppDb;
using LogicLib1.Services.AppSmtp;
using ToolsLib1.PaymentApi;
using static CommonLib1.Models.Constants;

namespace LogicLib1.Services.AppPayment;

public class AppPaymentApi (
             IToolPaymentApi _paymentApi,
             IAppEmailer     _appEmailer,
             IAppDbOperator  _appDbOperator) : IAppPaymentApi
{
    public async Task<string> ProcessClientPaymentAsync(string paymentIntentId, ClientRequest req)
    {
        var res = await _paymentApi.GetPaymentIntentStatusAsync(paymentIntentId);

        if (res == "succeeded")
        {
            await _appDbOperator.PatchClientStatusAsync(req.ClientInformation.ClientBookingId, ClientStatus.Paid);

            await _appDbOperator.PostClientApptSchedAsync(req);

            await _appEmailer.SendEmailAsync(req);
        }

        return res;
    }
}
