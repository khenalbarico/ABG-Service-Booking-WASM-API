namespace CommonLib1.Models.Schedules;

public sealed class ApptSchedBooking
{
    public string                 ClientBookingId { get; set; } = "";
    public List<ApptSchedService> Services        { get; set; } = [];
}
