namespace CommonLib1.Models.Schedules;

public class ScheduleCfg
{
    public List<string>            StoreHours                           { get; set; } = [];

    public Dictionary<string, int> NailsAccommodationCapacities         { get; set; } = [];

    public Dictionary<string, int> OtherServicesAccommodationCapacities { get; set; } = [];
}
