using CommonLib1.Models.Client;
using LogicLib1.Services.AppDb;
using TestProject1.TestHelpers;
using Xunit.Abstractions;
using static CommonLib1.Models.Constants;

namespace TestProject1.AppDbOperatorFacts1;

public class ClientReqAdder1(ITestOutputHelper _ctx)
{
    [Fact] public async Task Add_Client_Req()
    {
        var payload = new ClientRequest
        {
            ClientInformation = new ClientInformation
            {
                ClientBookingId = "PeterBookingId3",
                Email = "sample3@test.com",
                ContactNumber = "0961454252",
                FirstName = "Khen3",
                LastName = "Albarico3",
                BookingDate = DateTime.Now
            },
            ClientServices =
            [
                new() {
                    ServiceUid = "ServiceUid3",
                    ServiceName = "Nails",
                    ServiceDesign = "Complex",
                    ServiceDetails = "SOFTGEL EXTENSION",
                    ServiceCost = 1,
                    Branch = ServiceBranch.Anabu,
                    ServiceDate = new DateTime(2026, 4, 10, 10, 0, 0)
                }
            ],
            Status = ClientStatus.Pending
        };

        var _sut = _ctx.Get<IAppDbOperator>();

        await _sut.PostClientRequestAsync(payload);
    }
}
