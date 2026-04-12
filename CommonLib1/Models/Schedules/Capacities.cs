namespace CommonLib1.Models.Schedules;

public static class Capacities
{
    public static readonly Dictionary<string, int> NailSlotCapacities = new(StringComparer.OrdinalIgnoreCase)
    {
        ["10:00 AM"] = 3,
        ["11:00 AM"] = 3,
        ["12:00 PM"] = 4,
        ["1:00 PM"] = 1,
        ["1:30 PM"] = 2,
        ["2:00 PM"] = 2,
        ["2:30 PM"] = 1,
        ["3:00 PM"] = 2,
        ["3:30 PM"] = 1,
        ["4:00 PM"] = 2,
        ["4:30 PM"] = 1,
        ["5:00 PM"] = 2,
        ["6:00 PM"] = 4,
        ["7:00 PM"] = 2,
        ["7:30 PM"] = 1,
        ["8:00 PM"] = 2,
        ["8:30 PM"] = 2,
        ["9:30 PM"] = 4
    };
}
