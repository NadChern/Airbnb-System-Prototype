using AirbnbREST.Models;
using AirbnbREST.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AirbnbREST.Controllers;

[ApiController]
    [Route("api/availability")]
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IPropertyRepository _propertyRepository;

        public AvailabilityController(IAvailabilityRepository availabilityRepository, IPropertyRepository propertyRepository)
        {
            _availabilityRepository = availabilityRepository;
            _propertyRepository = propertyRepository;
        }

        // Get availability by property ID
        [HttpGet("{propertyId}")]
        public async Task<IEnumerable<Availability>> GetAvailability(Guid propertyId)
        {
            return await _availabilityRepository.GetAvailabilitiesByPropertyIdAsync(propertyId);
        }

        // Add availability for a property
        [HttpPost]
        public async Task<ActionResult<Availability>> AddAvailability(Availability availability)
        {
            var property = await _propertyRepository.GetByIdAsync(availability.PropertyId);
            if (property == null)
                return BadRequest("Property does not exist.");

            await _availabilityRepository.AddAsync(availability);
            return CreatedAtAction(nameof(GetAvailability), new { propertyId = availability.PropertyId }, availability);
        }

        // Update an existing availability entry
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAvailability(Guid id, Availability updatedAvailability)
        {
            var existingAvailability = await _availabilityRepository.GetByIdAsync(id);
            if (existingAvailability == null)
                return NotFound("Availability record not found.");

            // Update only provided fields
            existingAvailability.StartDate = updatedAvailability.StartDate != default ? updatedAvailability.StartDate : existingAvailability.StartDate;
            existingAvailability.EndDate = updatedAvailability.EndDate != default ? updatedAvailability.EndDate : existingAvailability.EndDate;

            await _availabilityRepository.UpdateAsync(existingAvailability);
            return NoContent();
        }

        // Delete an availability record
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAvailability(Guid id)
        {
            var availability = await _availabilityRepository.GetByIdAsync(id);
            if (availability == null)
                return NotFound("Availability record not found.");

            await _availabilityRepository.DeleteAsync(id);
            return NoContent();
        }
    }