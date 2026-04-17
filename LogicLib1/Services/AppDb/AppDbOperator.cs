using CommonLib1.Models.__Base__;
using CommonLib1.Models.Client;
using CommonLib1.Models.Schedules;
using CommonLib1.Models.Service;
using LogicLib1.Helpers.Schedule;
using LogicLib1.Services.AppBooking;
using ToolsLib1.FirebaseTools;
using static CommonLib1.Models.Constants;

namespace LogicLib1.Services.AppDb;

public class AppDbOperator(
    IToolFirebaseDbOperations _dbOperations,
    IBookingCapacity          _bookingCapacity) : IAppDbOperator
{
    public async Task<ServiceCollectionResp> GetServicesAsync()
    {
        var nails    = await _dbOperations.GetListAsync<NailsService>("Services", "Nails");
        var lashes   = await _dbOperations.GetListAsync<LashesService>("Services", "Lash");
        var eyebrows = await _dbOperations.GetListAsync<EyebrowsService>("Services", "Eyebrows");
        var footspa  = await _dbOperations.GetListAsync<FootspaService>("Services", "Footspa");
        var pedicure = await _dbOperations.GetListAsync<PedicureService>("Services", "Pedicure");

        return new ServiceCollectionResp
        {
            Nails    = nails,
            Lashes   = lashes,
            Eyebrows = eyebrows,
            Footspa  = footspa,
            Pedicure = pedicure
        };
    }

    public async Task PostClientRequestAsync(ClientRequest req)
    {
        await _bookingCapacity.ValidateAvailabilityAsync(req);

        var bookingId = req.ClientInformation.ClientBookingId;

        await _dbOperations.PutAsync(req, "ClientRequests", bookingId);
    }

    public async Task PostClientApptSchedAsync(ClientRequest req)
    {
        var bookingId       = req.ClientInformation.ClientBookingId;

        var groupedServices = (req.ClientServices ?? []).GroupBy(x => x.ServiceDate);

        foreach (var group in groupedServices)
        {
            var serviceDateKey = group.Key.ToServiceDateKey();

            var scheduleEntry = new ApptSchedBooking
            {
                ClientBookingId = bookingId,
                Services        = [.. group.Select(service => new ApptSchedService
                {
                    ServiceName    = service.ServiceName,
                    ServiceDesign  = service.ServiceDesign,
                    ServiceDetails = service.ServiceDetails,
                    Branch         = service.Branch
                })]
            };

            await _dbOperations.PutAsync(
                scheduleEntry,
                "AppointmentSchedules",
                serviceDateKey,
                bookingId
            );
        }
    }

    public async Task PatchClientStatusAsync(string bookingId, ClientStatus status)
    {
        await _dbOperations.PatchFieldsAsync(
            new Dictionary<string, object?>
            {
                ["Status"] = status
            },
            "ClientRequests",
            bookingId);
    }

    public async Task DeleteServiceAsync(string category, string serviceUid)
    {
        switch (category.Trim())
        {
            case "Nails":
                await _dbOperations.DeleteAsync("Services", "Nails", serviceUid);
                break;

            case "Lash":
                await _dbOperations.DeleteAsync("Services", "Lash", serviceUid);
                break;

            case "Eyebrows":
                await _dbOperations.DeleteAsync("Services", "Eyebrows", serviceUid);
                break;

            case "Footspa":
                await _dbOperations.DeleteAsync("Services", "Footspa", serviceUid);
                break;

            case "Pedicure":
                await _dbOperations.DeleteAsync("Services", "Pedicure", serviceUid);
                break;

            default:
                throw new InvalidOperationException($"Unsupported service category: {category}");
        }
    }

    public async Task SaveServiceAsync(string category, BaseSvcStructure service)
    {
        switch (category.Trim())
        {
            case "Nails":
                await _dbOperations.PutAsync(new NailsService
                {
                    Uid           = service.Uid,
                    Details       = service.Details,
                    Cost          = service.Cost,
                    Designs       = service.Designs ?? [],
                    ScheduleSlots = service.ScheduleSlots ?? new BaseSchedSlot()
                }, "Services", "Nails", service.Uid);
                break;

            case "Lash":
                await _dbOperations.PutAsync(new LashesService
                {
                    Uid           = service.Uid,
                    Details       = service.Details,
                    Cost          = service.Cost,
                    Designs       = service.Designs ?? [],
                    ScheduleSlots = service.ScheduleSlots ?? new BaseSchedSlot()
                }, "Services", "Lash", service.Uid);
                break;

            case "Eyebrows":
                await _dbOperations.PutAsync(new EyebrowsService
                {
                    Uid           = service.Uid,
                    Details       = service.Details,
                    Cost          = service.Cost,
                    Designs       = service.Designs ?? [],
                    ScheduleSlots = service.ScheduleSlots ?? new BaseSchedSlot()
                }, "Services", "Eyebrows", service.Uid);
                break;

            case "Footspa":
                await _dbOperations.PutAsync(new FootspaService
                {
                    Uid           = service.Uid,
                    Details       = service.Details,
                    Cost          = service.Cost,
                    Designs       = service.Designs ?? [],
                    ScheduleSlots = service.ScheduleSlots ?? new BaseSchedSlot()
                }, "Services", "Footspa", service.Uid);
                break;

            case "Pedicure":
                await _dbOperations.PutAsync(new PedicureService
                {
                    Uid           = service.Uid,
                    Details       = service.Details,
                    Cost          = service.Cost,
                    Designs       = service.Designs ?? [],
                    ScheduleSlots = service.ScheduleSlots ?? new BaseSchedSlot()
                }, "Services", "Pedicure", service.Uid);
                break;

            default:
                throw new InvalidOperationException($"Unsupported service category: {category}");
        }
    }

    public async Task<List<ApptSchedRec>> GetAppointmentSchedulesAsync()
    {
        var rawSchedules = await _dbOperations.GetAsync
            <Dictionary<string,
             Dictionary<string,
             ApptSchedBooking>>>("AppointmentSchedules")
                           ?? [];

        var result = new List<ApptSchedRec>();

        foreach (var scheduleDate in rawSchedules)
        {
            if (!scheduleDate.Key.TryParseServiceDateKey(out var parsedDate))
                continue;

            var bookings = scheduleDate.Value ?? [];

            foreach (var booking in bookings)
            {
                var entry = booking.Value ?? new ApptSchedBooking();

                result.Add(new ApptSchedRec
                {
                    ServiceDateKey  = scheduleDate.Key,
                    ServiceDate     = parsedDate,
                    ClientBookingId = string.IsNullOrWhiteSpace(entry.ClientBookingId) ? booking.Key : entry.ClientBookingId,
                    Services        = entry.Services ?? []
                });
            }
        }

        return result;
    }

    public async Task<List<ClientRequest>> GetClientRequestsAsync()
    => await _dbOperations.GetListAsync<ClientRequest>("ClientRequests");

    public async Task PostScheduleCfgAsync(ScheduleCfg cfg)
    => await _dbOperations.PutAsync(cfg, "ScheduleConfig");
    
    public async Task<ScheduleCfg> GetScheduleCfgAsync()
    => await _dbOperations.GetAsync<ScheduleCfg>("ScheduleConfig");
    
}