using CommonLib1.Models.__Base__;
using CommonLib1.Models.Client;
using CommonLib1.Models.Schedules;
using CommonLib1.Models.Service;
using static CommonLib1.Models.Constants;

namespace LogicLib1.Services.AppDb;

public interface IAppDbOperator
{
    Task<ServiceCollectionResp> GetServicesAsync();
    Task PostClientRequestAsync(ClientRequest req);
    Task PostClientApptSchedAsync(ClientRequest req);
    Task PatchClientStatusAsync(string bookingId, ClientStatus status);
    Task<List<ClientRequest>> GetClientRequestsAsync();
    Task DeleteServiceAsync(string category, string serviceUid);
    Task<List<ApptSchedRec>> GetAppointmentSchedulesAsync();
    Task SaveServiceAsync(string category, BaseSvcStructure service);
    Task PostScheduleCfgAsync(ScheduleCfg cfg);
    Task<ScheduleCfg> GetScheduleCfgAsync();
}