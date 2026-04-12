using CommonLib1.Models.Client;
using System.Text.Json;
using ToolsLib1.HttpClientTools;
using ToolsLib1.PaymentApi.PaymongoModels;

namespace ToolsLib1.PaymentApi;

public sealed class PaymongoQrph(IPaymongoCfg _cfg) : IToolPaymentApi
{
    readonly HttpClient _httpClient = _cfg.CreatePaymongoClient();

    public async Task<PaymongoQrphChargeResult> CreateQrphChargeAsync(
                 ClientRequest payload,
                 CancellationToken ct = default)
    {
        var totalAmountPhp      = payload.ClientServices.Sum(x => x.ServiceCost);
        var totalAmountCentavos = (int)Math.Round(totalAmountPhp * 100m, MidpointRounding.AwayFromZero);
        string desc             = BuildDescription(payload);

        var metadata = new Dictionary<string, string>
        {
            ["client_booking_id"] = payload.ClientInformation.ClientBookingId ?? "",
            ["customer_name"]     = $"{payload.ClientInformation.FirstName} {payload.ClientInformation.LastName}".Trim(),
            ["customer_email"]    = payload.ClientInformation.Email ?? "",
            ["service_count"]     = payload.ClientServices.Count.ToString(),
            ["services"]          = string.Join(", ", payload.ClientServices.Select(s => $"{s.ServiceUid}:{s.ServiceName}"))
        };

        var paymentIntentReq = new
        {
            data = new
            {
                attributes = new
                {
                    amount                 = totalAmountCentavos,
                    currency               = "PHP",
                    capture_type           = "automatic",
                    payment_method_allowed = new[] { "qrph" },
                    description            = desc,
                    statement_descriptor   = "BOOKING",
                    metadata
                }
            }
        };

        var paymentIntentResp = await _httpClient.PostAsync<PaymongoEnvelope<PaymentIntentData>>(
            "payment_intents",
            paymentIntentReq,
            ct);

        var paymentIntentId = paymentIntentResp.Data.Id;

        var paymentMethodReq = new
        {
            data = new
            {
                attributes = new
                {
                    type = "qrph",
                    billing = new
                    {
                        name = $"{payload.ClientInformation.FirstName} {payload.ClientInformation.LastName}".Trim(),
                        email = payload.ClientInformation.Email,
                        phone = payload.ClientInformation.ContactNumber,
                        address = new
                        {
                            line1 = "N/A",
                            line2 = "N/A",
                            city = "N/A",
                            state = "N/A",
                            postal_code = "0000",
                            country = "PH"
                        }
                    }
                }
            }
        };

        var paymentMethodResp = await _httpClient.PostAsync<PaymongoEnvelope<PaymentMethodData>>(
            "payment_methods",
            paymentMethodReq,
            ct);

        var paymentMethodId = paymentMethodResp.Data.Id;

        var attachReq = new
        {
            data = new
            {
                attributes = new
                {
                    payment_method = paymentMethodId,
                    client_key = paymentIntentResp.Data.Attributes.ClientKey
                }
            }
        };

        var attachResp = await _httpClient.PostAsync<PaymongoEnvelope<PaymentIntentData>>(
           $"payment_intents/{paymentIntentId}/attach",
           attachReq,
           ct);

        var nextAction = attachResp.Data.Attributes.NextAction;
        var qrCode = nextAction?.Code;

        return new PaymongoQrphChargeResult
        {
            PaymentIntentId     = attachResp.Data.Id,
            PaymentIntentStatus = attachResp.Data.Attributes.Status,
            PaymentMethodId     = paymentMethodId,
            AmountCentavos      = attachResp.Data.Attributes.Amount,
            AmountPhp           = attachResp.Data.Attributes.Amount / 100m,
            QrImageUrl          = qrCode?.ImageUrl,
            QrCodeId            = qrCode?.Id,
            QrLabel             = qrCode?.Label,
            NextActionType      = nextAction?.Type,
            RawResponse         = JsonSerializer.Serialize(attachResp)
        };
    }

    public static string BuildDescription(ClientRequest payload)
    {
        var services = string.Join(
            ", ",
            payload.ClientServices.Select(x => $"{x.ServiceName} ({x.ServiceDetails})"));

        return $"Appointent booking {payload.ClientInformation.ClientBookingId}: {services}";
    }

    public async Task<string> GetPaymentIntentStatusAsync(string paymentIntentId, CancellationToken ct = default)
    {
        var resp = await _httpClient.GetAsync<PaymongoEnvelope<PaymentIntentData>>(
            $"payment_intents/{paymentIntentId}",
            ct);

        return resp.Data.Attributes.Status;
    }
}