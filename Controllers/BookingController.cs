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

    public BookingController(IBookingRepository bookingRepository, IPropertyRepository propertyRepository)
    {
        _bookingRepository = bookingRepository;
        _propertyRepository = propertyRepository;
    }

    // Create a new booking
    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
    {
        var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);
        if (property == null)
            return BadRequest("Property does not exist.");

        var isAvailable = await _bookingRepository.IsPropertyAvailableAsync(booking.PropertyId, booking.StartDate, booking.EndDate);
        if (!isAvailable)
            return BadRequest("Property is not available for the selected dates.");

        await _bookingRepository.AddAsync(booking);
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    // Get booking by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBookingById(Guid id)
    {
        var booking = await _bookingRepository.GetByIdAsync(id);
        if (booking == null)
            return NotFound("Booking not found.");

        return Ok(booking);
    }

    // Get bookings by guest ID
    [HttpGet("user/{userId}")]
    public async Task<IEnumerable<Booking>> GetBookingsByUser(Guid userId)
    {
        return await _bookingRepository.GetBookingsByGuestIdAsync(userId);
    }

    // Get bookings by property ID
    [HttpGet("property/{propertyId}")]
    public async Task<IEnumerable<Booking>> GetBookingsByProperty(Guid propertyId)
    {
        return await _bookingRepository.GetBookingsByPropertyIdAsync(propertyId);
    }

    // Confirm a booking (new feature)
    [HttpPut("{id}/confirm")]
    public async Task<ActionResult> ConfirmBooking(Guid id)
    {
        var success = await _bookingRepository.ConfirmBookingAsync(id);
        if (!success)
            return BadRequest("Booking cannot be confirmed (either it does not exist, is already confirmed, or is unavailable).");

        return NoContent();
    }

    // Cancel a booking
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult> CancelBooking(Guid id)
    {
        var success = await _bookingRepository.CancelBookingAsync(id);
        if (!success)
            return BadRequest("Booking cannot be cancelled (either it does not exist, is not confirmed, or is already cancelled).");

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

