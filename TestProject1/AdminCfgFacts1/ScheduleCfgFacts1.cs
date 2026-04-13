using CommonLib1.Models.Schedules;
using LogicLib1.Services.AppDb;
using TestProject1.TestHelpers;
using Xunit.Abstractions;

namespace TestProject1.AdminCfgFacts1;

public class ScheduleCfgFacts1 (ITestOutputHelper _ctx)
{
    [Fact] public async Task Add_Schedule_Configuration()
    {
        var scheduleCfg = new ScheduleCfg
        {
            StoreHours                          = ["10-11 AM", "11-12 AM", "12-1 PM"],
            NailsAccommodationCapacities = new Dictionary<string, int>
            {
                ["10-11 AM"] = 2,
                ["11-12 AM"] = 3,
                ["12-1 PM"] = 4
            },
            OtherServicesAccommodationCapacities = new Dictionary<string, int>
            {
                ["10-11 AM"] = 3,
                ["11-12 AM"] = 3,
                ["12-1 PM"] = 3
            }
        };

        var _sut = _ctx.Get<IAppDbOperator>();

        await _sut.PostScheduleCfgAsync(scheduleCfg);
    }

    [Fact] public async Task Get_Schedule_Configuration()
    {
        var _sut = _ctx.Get<IAppDbOperator>();

        var res  = await _sut.GetScheduleCfgAsync();
    }
}
