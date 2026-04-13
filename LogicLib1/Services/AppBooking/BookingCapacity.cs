using CommonLib1.Models.Client;
using CommonLib1.Models.Schedules;
using LogicLib1.Helpers.Schedule;
using ToolsLib1.FirebaseTools;

namespace LogicLib1.Services.AppBooking;

public class BookingCapacity (IToolFirebaseDbOperations _dbOperations) : IBookingCapacity
{
    public async Task ValidateAvailabilityAsync(ClientRequest req)
    {
        var scheduleCfg   = await _dbOperations.GetAsync<ScheduleCfg>("ScheduleConfig")
            ?? throw new InvalidOperationException("Schedule configuration was not found.");

        var groupedByDate = req.ClientServices.GroupBy(x => x.ServiceDate);

        foreach (var dateGroup in groupedByDate)
        {
            var serviceDate    = dateGroup.Key;
            var serviceDateKey = serviceDate.ToServiceDateKey();

            var existingBookings = await _dbOperations.GetAsync<Dictionary<string, ApptSchedBooking>>(
                "AppointmentSchedules",
                serviceDateKey
            ) ?? [];

            var requestedBuckets = dateGroup
                .Select(GetBucketFromClientService)
                .Distinct();

            foreach (var bucket in requestedBuckets)
            {
                var capacities      = GetCapacities(scheduleCfg, bucket);
                var slotKey         = serviceDate.ResolveSlotKey(capacities);
                var allowedCapacity = capacities[slotKey];
                var existingCount   = CountExistingBookings(existingBookings, bucket);

                if (existingCount >= allowedCapacity)
                    throw new InvalidOperationException($"The booking slot is full for {bucket} on {serviceDate:MMMM dd, yyyy hh:mm tt}.");
            }
        }
    }

    static int CountExistingBookings(
            Dictionary<string, ApptSchedBooking> existingBookings,
            BookingServiceBucket                 bucket)
    => existingBookings.Values.Count(x => x != null && BookingHasBucket(x, bucket));
    
    static bool BookingHasBucket(ApptSchedBooking booking, BookingServiceBucket bucket)
    {
        var services = booking.Services ?? [];

        return bucket switch
        {
            BookingServiceBucket.Nails => services.Any(IsNailsApptSchedService),
            BookingServiceBucket.OtherServices => services.Any(x => !IsNailsApptSchedService(x)),
            _ => false
        };
    }

    static BookingServiceBucket GetBucketFromClientService(ClientService service)
    {
        return IsNailsClientService(service)
            ? BookingServiceBucket.Nails
            : BookingServiceBucket.OtherServices;
    }

    static bool IsNailsClientService(ClientService service)
    => string.Equals(service.ServiceName, "Nails", StringComparison.OrdinalIgnoreCase);
    
    static bool IsNailsApptSchedService(ApptSchedService service)
    => string.Equals(service.ServiceName, "Nails", StringComparison.OrdinalIgnoreCase);
    
    static Dictionary<string, int> GetCapacities(ScheduleCfg scheduleCfg, BookingServiceBucket bucket)
    {
        var capacities = bucket switch
        {
            BookingServiceBucket.Nails => scheduleCfg.NailsAccommodationCapacities,
            BookingServiceBucket.OtherServices => scheduleCfg.OtherServicesAccommodationCapacities,
            _ => null
        };

        if (capacities == null || capacities.Count == 0)
            throw new InvalidOperationException($"Accommodation capacities are not configured for {bucket}.");

        return capacities;
    }
}