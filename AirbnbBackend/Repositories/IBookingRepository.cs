using AirbnbREST.Models;

namespace AirbnbREST.Repositories;

public interface IBookingRepository : IRepository<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsByGuestIdAsync(Guid guestId);
    Task<IEnumerable<Booking>> GetBookingsByPropertyIdAsync(Guid propertyId);
    Task<bool> IsPropertyAvailableAsync(Guid propertyId, DateTime startDate, DateTime endDate);
    Task<bool> ConfirmBookingAsync(Guid bookingId);
    Task<bool> CancelBookingAsync(Guid bookingId);

}