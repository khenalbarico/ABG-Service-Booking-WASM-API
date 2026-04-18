using CommonLib1.Models.Client;
using LogicLib1.Services.AppDb;
using static CommonLib1.Models.Constants;

namespace TestProject1.TestHelpers;

internal class ClientReqPayloadHelper
{
    public static ClientRequest CreatePayload(
           string   bookingId,
           string   firstName,
           DateTime serviceDate)
    {
        return new ClientRequest
        {
            ClientInformation = new ClientInformation
            {
                ClientBookingId = bookingId,
                Email           = "sample@test.com",
                ContactNumber   = "0961454252",
                FirstName       = firstName,
                LastName        = "Albarico",
                BookingDate     = DateTime.Now
            },
            ClientServices =
            [
                new ClientService
                {
                    ServiceUid     = "ServiceUid",
                    ServiceName    = "Nails",
                    ServiceDesign  = "",
                    ServiceDetails = "Sample1",
                    ServiceCost    = 1,
                    Branch         = ServiceBranch.Manila,
                    ServiceDate    = serviceDate
                }
            ],
            Status = ClientStatus.Pending
        };
    }

    public static async Task<string> TryAddAsync(
           IAppDbOperator sut,
           ClientRequest  payload)
    {
        try
        {
            await sut.PostClientRequestAsync(payload);
            return $"SUCCESS: {payload.ClientInformation.ClientBookingId}";
        }
        catch (Exception ex)
        {
            return $"FAILED: {payload.ClientInformation.ClientBookingId} | {ex.Message}";
        }
    }
}
