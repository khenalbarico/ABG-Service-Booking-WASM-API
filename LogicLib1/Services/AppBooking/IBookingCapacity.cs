using CommonLib1.Models.Client;

namespace LogicLib1.Services.AppBooking;

public interface IBookingCapacity
{
    Task ValidateAvailabilityAsync(ClientRequest request);
}