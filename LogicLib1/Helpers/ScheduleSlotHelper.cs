using CommonLib1.Models.Client;
using CommonLib1.Models.Schedules;
using System.Globalization;
using static CommonLib1.Models.Constants;

namespace LogicLib1.Helpers;

public static class ScheduleSlotHelper
{
    public static string ToServiceDateKey(this DateTime value)
    => value.ToString("yyyy-MM-ddTHH:mm:ss");
    
    public static bool TryParseServiceDateKey(this string value, out DateTime result)
    {
        return DateTime.TryParseExact(
            value,
            "yyyy-MM-ddTHH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out result);
    }

    public static string ResolveSlotKey(this DateTime serviceDate, Dictionary<string, int> capacities)
    {
        foreach (var entry in capacities)
        {
            if (IsMatchingSlot(serviceDate, entry.Key))
                return entry.Key;
        }

        throw new InvalidOperationException(
            $"No matching capacity slot found for {serviceDate:yyyy-MM-dd hh:mm tt}.");
    }

    public static List<ClientRequest> GetValidRequests(
                 this List<ClientRequest> existingRequests,
                      string              bookingId,
                      DateTime            now,
                      TimeSpan            holdDuration,
                      out List<string>    expiredIds)
    {
        var valid = new List<ClientRequest>();
        expiredIds = [];

        foreach (var existing in existingRequests)
        {
            var existingId = existing.ClientInformation.ClientBookingId;

            if (existingId == bookingId)
                continue;

            if (IsExpiredPending(existing, now, holdDuration))
            {
                expiredIds.Add(existingId);
                continue;
            }

            valid.Add(existing);
        }

        return valid;
    }

    public static void ValidateSlots(
            this List<ClientService> incomingServices,
                 List<ClientRequest> validRequests,
                 ScheduleCfg         cfg)
    {
        foreach (var svc in incomingServices)
        {
            var isNails = IsNailsService(svc);

            var capacities = isNails
                ? cfg.NailsAccommodationCapacities ?? []
                : cfg.OtherServicesAccommodationCapacities ?? [];

            var slotKey = svc.ServiceDate.ResolveSlotKey(capacities);

            var capacity = capacities.TryGetValue(slotKey, out var cap) ? cap : 0;

            var existingCount = validRequests
                .SelectMany(x => x.ClientServices ?? [])
                .Count(x => IsSameSlot(x, svc, isNails, capacities, slotKey));

            var incomingCount = incomingServices
                .Count(x => IsSameSlot(x, svc, isNails, capacities, slotKey));

            if (existingCount + incomingCount > capacity)
                throw new InvalidOperationException(
                    "The date is currently on queue, please try again booking this date after 2 mins.");
        }
    }

    private static bool IsExpiredPending(
            ClientRequest request,
            DateTime      now,
            TimeSpan      holdDuration)
    {
        return request.Status == ClientStatus.Pending &&
               now - request.ClientInformation.BookingDate >= holdDuration;
    }

    private static bool IsNailsService(ClientService svc)
    {
        return svc.ServiceName?.Contains("nail", StringComparison.OrdinalIgnoreCase) == true;
    }

    private static bool IsSameSlot(
        ClientService           service,
        ClientService           target,
                   bool         isNails,
        Dictionary<string, int> capacities,
                   string       slotKey)
    {
        var sameDate = service.ServiceDate.Date == target.ServiceDate.Date;
        var sameType = IsNailsService(service) == isNails;

        if (!sameDate || !sameType)
            return false;

        try
        {
            return service.ServiceDate.ResolveSlotKey(capacities) == slotKey;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsMatchingSlot(DateTime serviceDate, string slotKey)
    {
        if (!DateTime.TryParse(slotKey, CultureInfo.InvariantCulture, DateTimeStyles.None, out var slotTime))
            return false;

        return serviceDate.Hour == slotTime.Hour;
    }
}