using LogicLib1.Services.AppDb;
using TestProject1.TestHelpers;
using Xunit.Abstractions;
using static CommonLib1.Models.Constants;

namespace TestProject1.AppDbOperatorFacts1;

public class ClientReqAdder1(ITestOutputHelper _ctx)
{
    [Fact]
    public async Task Add_3_Client_Req_At_The_Same_Time()
    {
        var sut = _ctx.Get<IAppDbOperator>();

        var serviceDate = new DateTime(2026, 4, 17, 8, 0, 0);

        var payload1 = ClientReqPayloadHelper.CreatePayload("PeterBookingId101", "Peter", serviceDate);
        var payload2 = ClientReqPayloadHelper.CreatePayload("PeterBookingId102", "Juan", serviceDate);
        var payload3 = ClientReqPayloadHelper.CreatePayload("PeterBookingId103", "Maria", serviceDate);

        var tasks = new[]
        {
            ClientReqPayloadHelper.TryAddAsync(sut, payload1),
            ClientReqPayloadHelper.TryAddAsync(sut, payload2),
            ClientReqPayloadHelper.TryAddAsync(sut, payload3)
        };

        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
            _ctx.WriteLine(result);

        var successCount = results.Count(x => x.StartsWith("SUCCESS"));
        var failedCount  = results.Count(x => x.StartsWith("FAILED"));

        _ctx.WriteLine($"Success: {successCount}");
        _ctx.WriteLine($"Failed: {failedCount}");
    }


    [Fact]
    public async Task Get_Client_Req()
    {
        var sut = _ctx.Get<IAppDbOperator>();
        var res = await sut.GetClientRequestsAsync();

        foreach (var item in res)
            _ctx.WriteLine($"{item.ClientInformation.ClientBookingId} | {item.Status}");
    }

    [Fact]
    public async Task Patch_Client_Status()
    {
        var sut = _ctx.Get<IAppDbOperator>();

        var bookingId = "PeterBookingId2";
        var status = ClientStatus.Paid;

        await sut.PatchClientStatusAsync(bookingId, status);
    }
}