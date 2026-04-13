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
            if (IsWithinSlot(serviceDate, entry.Key))
                return entry.Key;
        }

        throw new InvalidOperationException($"No matching capacity slot found for {serviceDate:yyyy-MM-dd hh:mm tt}.");
    }

    private static bool IsWithinSlot(DateTime serviceDate, string slotKey)
    {
        var parts = slotKey.Split('-', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length != 2)
            return false;

        if (!DateTime.TryParse(parts[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out var start))
            return false;

        if (!DateTime.TryParse(parts[1], CultureInfo.InvariantCulture, DateTimeStyles.None, out var end))
            return false;

        var bookingTime = serviceDate.TimeOfDay;
        var startTime = start.TimeOfDay;
        var endTime = end.TimeOfDay;

        return bookingTime >= startTime && bookingTime < endTime;
    }
}