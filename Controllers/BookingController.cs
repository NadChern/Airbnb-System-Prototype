using AirbnbREST.Middleware;
using AirbnbREST.Models;
using AirbnbREST.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AirbnbREST.Controllers;

[ApiController]
[Route("api/bookings")]
public class BookingController : ControllerBase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserRepository _userRepository;

    public BookingController(IBookingRepository bookingRepository,
        IPropertyRepository propertyRepository, IUserRepository userRepository)
    {
        _bookingRepository = bookingRepository;
        _propertyRepository = propertyRepository;
        _userRepository = userRepository;
    }

    // Create a new booking
    // Protect this route → Only logged-in guests can create a booking
    [RequireLogin]
    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var loggedInUserRole = await _userRepository.GetUserRoleAsync(loggedInUserId);

        if (loggedInUserRole != UserRole.Guest)
            return Forbid(); // Hosts cannot create bookings

        booking.GuestId = loggedInUserId; // Assign guest ID
        var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);
        if (property == null)
            return BadRequest("Property does not exist.");

        var isAvailable = await _bookingRepository.IsPropertyAvailableAsync(booking.PropertyId, booking.StartDate, booking.EndDate);
        if (!isAvailable)
            return BadRequest("Property is not available for the selected dates.");

        await _bookingRepository.AddAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    // Get booking by ID (retrieve one booking)
    [RequireLogin]
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBookingById(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
            return NotFound("Booking not found.");

        var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);
        var userRole = await _userRepository.GetUserRoleAsync(loggedInUserId);

        // Only guests who made the booking or hosts of the property can view it
        if (booking.GuestId != loggedInUserId && property?.Owner != loggedInUserId && userRole != UserRole.Host)
            return Forbid();

        return Ok(booking);
    }


    // Get all bookings by guest ID (quest can see only their own bookings)
    [RequireLogin]
    [HttpGet("user")]
    public async Task<IEnumerable<Booking>> GetBookingsByUser()
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        return await _bookingRepository.GetBookingsByGuestIdAsync(loggedInUserId);
    }

    // Get bookings by property ID
    [RequireLogin]
    [HttpGet("property/{propertyId}")]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByProperty(Guid propertyId)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var property = await _propertyRepository.GetByIdAsync(propertyId);

        if (property == null)
            return NotFound("Property not found.");

        // Only the property owner (host) can see bookings for their property
        if (property.Owner != loggedInUserId)
            return Forbid();

        return Ok(await _bookingRepository.GetBookingsByPropertyIdAsync(propertyId));
    }

    // Confirm a booking (new feature)
    [RequireLogin]
    [HttpPut("{id}/confirm")]
    public async Task<ActionResult> ConfirmBooking(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
            return NotFound("Booking not found.");

        var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);

        // Only the host who owns the property can confirm bookings
        if (property == null || property.Owner != loggedInUserId)
            return Forbid();

        var success = await _bookingRepository.ConfirmBookingAsync(id);
        if (!success)
            return BadRequest("Booking cannot be confirmed.");

        return NoContent();
    }

    // Cancel a booking
    [RequireLogin]
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> CancelBooking(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var booking = await _bookingRepository.GetByIdAsync(id);

        if (booking == null)
            return NotFound("Booking not found.");

        // Only the guest who made the booking can cancel it
        if (booking.GuestId != loggedInUserId)
            return Forbid();

        var success = await _bookingRepository.CancelBookingAsync(id);
        if (!success)
            return BadRequest("Booking cannot be cancelled.");

        return NoContent();
    }

    // Check property availability for given dates
    [HttpGet("availability")]
    public async Task<ActionResult<bool>> CheckPropertyAvailability(Guid propertyId, DateTime startDate, DateTime endDate)
    {
        var isAvailable = await _bookingRepository.IsPropertyAvailableAsync(propertyId, startDate, endDate);
        return Ok(isAvailable);
    }
}
