using AirbnbREST.Middleware;
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
    private readonly IUserRepository _userRepository;
    public AvailabilityController(IAvailabilityRepository availabilityRepository, 
        IPropertyRepository propertyRepository, IUserRepository userRepository)
    {
        _availabilityRepository = availabilityRepository;
        _propertyRepository = propertyRepository;
        _userRepository = userRepository;
    }

    // Get availability by property ID
    [HttpGet("{propertyId}")]
    public async Task<IEnumerable<Availability>> GetAvailability(Guid propertyId)
    {
        return await _availabilityRepository.GetAvailabilitiesByPropertyIdAsync(propertyId);
    }

    // Add availability for a property
    // Only logged-in hosts can add availability
    [RequireLogin]
    [HttpPost]
    public async Task<ActionResult<Availability>> AddAvailability(Availability availability)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var loggedInUserRole = await _userRepository.GetUserRoleAsync(loggedInUserId);

        // Guests cannot add availability
        if (loggedInUserRole != UserRole.Host)
            return Forbid(); // 403 Forbidden

        // Check if property exists
        var property = await _propertyRepository.GetByIdAsync(availability.PropertyId);
        if (property == null)
            return BadRequest("Property does not exist.");

        // Ensure the host owns this property
        if (property.Owner != loggedInUserId)
            return Forbid(); // 403 Forbidden (Host can only modify their own properties)

        await _availabilityRepository.AddAsync(availability);
        return CreatedAtAction(nameof(GetAvailability), new { propertyId = availability.PropertyId }, availability);
    }

    // Update an existing availability entry
    // Only logged-in hosts can update availability for their property
    [RequireLogin]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAvailability(Guid id, Availability updatedAvailability)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var loggedInUserRole = await _userRepository.GetUserRoleAsync(loggedInUserId);

        // Guests cannot update availability
        if (loggedInUserRole != UserRole.Host)
            return Forbid();

        var existingAvailability = await _availabilityRepository.GetByIdAsync(id);
        if (existingAvailability == null)
            return NotFound("Availability record not found.");

        // Get property and verify host ownership
        var property = await _propertyRepository.GetByIdAsync(existingAvailability.PropertyId);
        if (property == null || property.Owner != loggedInUserId)
            return Forbid(); // 403 Forbidden (Host can only modify their own properties)

        // Update only provided fields
        existingAvailability.StartDate = updatedAvailability.StartDate != default ? updatedAvailability.StartDate : existingAvailability.StartDate;
        existingAvailability.EndDate = updatedAvailability.EndDate != default ? updatedAvailability.EndDate : existingAvailability.EndDate;

        await _availabilityRepository.UpdateAsync(existingAvailability);
        return NoContent();
    }

    // Delete an availability record
    // Only logged-in **Hosts** can delete availability
    [RequireLogin]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAvailability(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var loggedInUserRole = await _userRepository.GetUserRoleAsync(loggedInUserId);

        // Guests cannot delete availability
        if (loggedInUserRole != UserRole.Host)
            return Forbid();

        var availability = await _availabilityRepository.GetByIdAsync(id);
        if (availability == null)
            return NotFound("Availability record not found.");

        // Get property and verify host ownership
        var property = await _propertyRepository.GetByIdAsync(availability.PropertyId);
        if (property == null || property.Owner != loggedInUserId)
            return Forbid(); // 403 Forbidden (Host can only modify their own properties)

        await _availabilityRepository.DeleteAsync(id);
        return NoContent();
    }
    
}
