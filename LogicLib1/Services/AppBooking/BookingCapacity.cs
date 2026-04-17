using CommonLib1.Models.Client;
using CommonLib1.Models.Schedules;
using LogicLib1.Helpers.Schedule;
using ToolsLib1.FirebaseTools;

namespace LogicLib1.Services.AppBooking;

public class BookingCapacity(IToolFirebaseDbOperations _dbOperations) : IBookingCapacity
{
    public async Task ValidateAvailabilityAsync(ClientRequest req)
    {
        var scheduleCfg = await _dbOperations.GetAsync<ScheduleCfg>("ScheduleConfig");

        var groupedByDate = req.ClientServices.GroupBy(x => x.ServiceDate);

        foreach (var dateGroup in groupedByDate)
        {
            var serviceDate    = dateGroup.Key;
            var serviceDateKey = serviceDate.ToServiceDateKey();

            ValidateExclusiveFootspaPedicureWithinRequest(dateGroup, serviceDate);

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
                    throw new InvalidOperationException(GetCapacityErrorMessage(bucket, serviceDate));
            }
        }
    }

    static void ValidateExclusiveFootspaPedicureWithinRequest(
        IGrouping<DateTime, ClientService> dateGroup,
        DateTime serviceDate)
    {
        if (dateGroup.Count(IsFootspaOrPedicureClientService) > 1)
        {
            throw new InvalidOperationException(
                $"Only one Footspa or Pedicure booking can be accommodated on {serviceDate:MMMM dd, yyyy hh:mm tt}.");
        }
    }

    static int CountExistingBookings(
        Dictionary<string, ApptSchedBooking> existingBookings,
        BookingServiceBucket bucket)
        => existingBookings.Values.Count(x => BookingHasBucket(x, bucket));

    static bool BookingHasBucket(ApptSchedBooking booking, BookingServiceBucket bucket)
    {
        var services = booking.Services;

        return bucket switch
        {
            BookingServiceBucket.Nails => services.Any(IsNailsApptSchedService),
            BookingServiceBucket.FootspaPedicureExclusive => services.Any(IsFootspaOrPedicureApptSchedService),
            BookingServiceBucket.OtherServices => services.Any(IsOtherServiceApptSchedService),
            _ => throw new NotImplementedException()
        };
    }

    static BookingServiceBucket GetBucketFromClientService(ClientService service)
    {
        if (IsNailsClientService(service))
            return BookingServiceBucket.Nails;

        if (IsFootspaOrPedicureClientService(service))
            return BookingServiceBucket.FootspaPedicureExclusive;

        return BookingServiceBucket.OtherServices;
    }

    static bool IsNailsClientService(ClientService service)
        => service.ServiceName.Equals("Nails", StringComparison.OrdinalIgnoreCase);

    static bool IsNailsApptSchedService(ApptSchedService service)
        => service.ServiceName.Equals("Nails", StringComparison.OrdinalIgnoreCase);

    static bool IsFootspaClientService(ClientService service)
        => service.ServiceName.Equals("Footspa", StringComparison.OrdinalIgnoreCase);

    static bool IsPedicureClientService(ClientService service)
        => service.ServiceName.Equals("Pedicure", StringComparison.OrdinalIgnoreCase);

    static bool IsFootspaOrPedicureClientService(ClientService service)
        => IsFootspaClientService(service) || IsPedicureClientService(service);

    static bool IsFootspaApptSchedService(ApptSchedService service)
        => service.ServiceName.Equals("Footspa", StringComparison.OrdinalIgnoreCase);

    static bool IsPedicureApptSchedService(ApptSchedService service)
        => service.ServiceName.Equals("Pedicure", StringComparison.OrdinalIgnoreCase);

    static bool IsFootspaOrPedicureApptSchedService(ApptSchedService service)
        => IsFootspaApptSchedService(service) || IsPedicureApptSchedService(service);

    static bool IsOtherServiceApptSchedService(ApptSchedService service)
        => !IsNailsApptSchedService(service) && !IsFootspaOrPedicureApptSchedService(service);

    static Dictionary<string, int> GetCapacities(ScheduleCfg scheduleCfg, BookingServiceBucket bucket)
        => bucket switch
        {
            BookingServiceBucket.Nails                    => scheduleCfg.NailsAccommodationCapacities,
            BookingServiceBucket.OtherServices            => scheduleCfg.OtherServicesAccommodationCapacities,
            BookingServiceBucket.FootspaPedicureExclusive => GetExclusiveFootspaPedicureCapacities(scheduleCfg),
            _                                             => throw new NotImplementedException()
        };

    static Dictionary<string, int> GetExclusiveFootspaPedicureCapacities(ScheduleCfg scheduleCfg)
        => scheduleCfg.OtherServicesAccommodationCapacities.ToDictionary(x => x.Key, _ => 1);

    static string GetCapacityErrorMessage(BookingServiceBucket bucket, DateTime serviceDate)
        => bucket switch
        {
            BookingServiceBucket.FootspaPedicureExclusive =>
                $"Footspa and Pedicure cannot be accommodated at the same date and time. The slot is already taken on {serviceDate:MMMM dd, yyyy hh:mm tt}.",

            _ =>
                $"The booking slot is full for {bucket} on {serviceDate:MMMM dd, yyyy hh:mm tt}."
        };
}