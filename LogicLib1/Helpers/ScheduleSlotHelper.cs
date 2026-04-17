using System.Globalization;

namespace LogicLib1.Helpers.Schedule;

public static class ScheduleSlotHelper
{
    public static string ToServiceDateKey(this DateTime value)
    {
        return value.ToString("yyyy-MM-ddTHH:mm:ss");
    }

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

    private static bool IsMatchingSlot(DateTime serviceDate, string slotKey)
    {
        if (!DateTime.TryParse(slotKey, CultureInfo.InvariantCulture, DateTimeStyles.None, out var slotTime))
            return false;

        return serviceDate.Hour == slotTime.Hour;
    }
}