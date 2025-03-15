using AirbnbREST.Middleware;
using AirbnbREST.Models;
using AirbnbREST.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AirbnbREST.Controllers
{
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

        // Create a new booking using hardcoded guest ID.
        [HttpPost]
        public async Task<ActionResult<Booking>> CreateBooking(Booking booking)
        {
            var dummyGuestId = Guid.Parse("e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            booking.GuestId = dummyGuestId;
            booking.Status = BookingStatus.Confirmed;

            var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);
            if (property == null)
                return BadRequest("Property does not exist.");

            var isAvailable = await _bookingRepository.IsPropertyAvailableAsync(booking.PropertyId, booking.StartDate, booking.EndDate);
            if (!isAvailable)
                return BadRequest("Property is not available for the selected dates.");

            await _bookingRepository.AddAsync(booking);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        // Get booking by ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBookingById(Guid id)
        {
            var dummyGuestId = Guid.Parse("e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);
            if (booking.GuestId != dummyGuestId && property?.Owner != dummyGuestId)
                return Forbid();

            return Ok(booking);
        }

        // Get all bookings by guest ID.
        [HttpGet("user")]
        public async Task<IEnumerable<Booking>> GetBookingsByUser()
        {
            var dummyGuestId = Guid.Parse("e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            return await _bookingRepository.GetBookingsByGuestIdAsync(dummyGuestId);
        }

        // Get bookings by property ID.
        [HttpGet("property/{propertyId}")]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookingsByProperty(Guid propertyId)
        {
            var dummyGuestId = Guid.Parse("e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            var property = await _propertyRepository.GetByIdAsync(propertyId);

            if (property == null)
                return NotFound("Property not found.");

            if (property.Owner != dummyGuestId)
                return Forbid();

            return Ok(await _bookingRepository.GetBookingsByPropertyIdAsync(propertyId));
        }

        // Confirm a booking.
        [HttpPut("{id}/confirm")]
        public async Task<ActionResult> ConfirmBooking(Guid id)
        {
            var dummyGuestId = Guid.Parse("e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            var property = await _propertyRepository.GetByIdAsync(booking.PropertyId);
            if (property == null || property.Owner != dummyGuestId)
                return Forbid();

            var success = await _bookingRepository.ConfirmBookingAsync(id);
            if (!success)
                return BadRequest("Booking cannot be confirmed.");

            return NoContent();
        }

        // Cancel a booking.
        [HttpPut("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(Guid id)
        {
            var dummyGuestId = Guid.Parse("e8e20f27-465f-491e-8c8f-3fd548ea9c14");
            var booking = await _bookingRepository.GetByIdAsync(id);

            if (booking == null)
                return NotFound("Booking not found.");

            // if (booking.GuestId != dummyGuestId)
            //     return Forbid();

            var success = await _bookingRepository.CancelBookingAsync(id);
            if (!success)
                return BadRequest("Booking cannot be cancelled.");

            return NoContent();
        }

        // Check property availability for given dates.
        [HttpGet("availability")]
        public async Task<ActionResult<bool>> CheckPropertyAvailability(Guid propertyId, DateTime startDate, DateTime endDate)
        {
            var isAvailable = await _bookingRepository.IsPropertyAvailableAsync(propertyId, startDate, endDate);
            return Ok(isAvailable);
        }
    }
}
