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
                ClientBookingId = "PeterBookingId",
                Email = "sample@test.com",
                ContactNumber = "0961454252",
                FirstName = "Khen",
                LastName = "Albarico",
                BookingDate = DateTime.Now
            },
            ClientServices =
            [
                new() {
                    ServiceUid = "ServiceUid",
                    ServiceName = "Footspa",
                    ServiceDesign = "",
                    ServiceDetails = "Sample Footpsa",
                    ServiceCost = 1,
                    Branch = ServiceBranch.Manila,
                    ServiceDate = new DateTime(2026, 4, 17, 15, 0, 0)
                }
            ],
            Status = ClientStatus.Pending
        };

        var _sut = _ctx.Get<IAppDbOperator>();

        await _sut.PostClientRequestAsync(payload);
    }

    [Fact] public async Task Get_Client_Req()
    {
        var _sut = _ctx.Get<IAppDbOperator>();

        var res  = _sut.GetClientRequestsAsync();
    }
}
