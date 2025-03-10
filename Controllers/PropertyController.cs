using AirbnbREST.DTOs;
using AirbnbREST.Middleware;
using AirbnbREST.Models;
using AirbnbREST.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AirbnbREST.Controllers;

[ApiController]
[Route("api/properties")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUserRepository _userRepository;

    public PropertyController(IPropertyRepository propertyRepository, 
        IUserRepository userRepository)
    {
        _propertyRepository = propertyRepository;
        _userRepository = userRepository;

    }

    // Get all properties
    [HttpGet]
    public async Task<IEnumerable<Property>> GetProperties()
    {
        return await _propertyRepository.GetAllAsync();
    }

    // Get property by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Property>> GetPropertyById(Guid id)
    {
        var property = await _propertyRepository.GetByIdAsync(id);
        if (property == null)
            return NotFound("Property not found.");

        return Ok(property);
    }

    // Search properties with filters (title, city, bedrooms, price, availability, etc.)
    [HttpGet("search")]
    public async Task<IEnumerable<Property>> SearchProperties([FromQuery] PropertySearchDto searchParams)
    {
        return await _propertyRepository.SearchPropertiesAsync(searchParams);
    }

    // Get properties by owner ID
    [HttpGet("owner/{ownerId}")]
    public async Task<IEnumerable<Property>> GetPropertiesByOwner(Guid ownerId)
    {
        return await _propertyRepository.GetPropertiesByOwnerIdAsync(ownerId);
    }

    // Only host can create/update/delete properties
    // Create new property
    [RequireLogin]
    [HttpPost]
    public async Task<ActionResult<Property>> CreateProperty(Property property)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var loggedInUserRole = await _userRepository.GetUserRoleAsync(loggedInUserId);

        if (loggedInUserRole != UserRole.Host)
            return Forbid(); // Only hosts can create properties

        property.Owner = loggedInUserId;
        await _propertyRepository.AddAsync(property);
        return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, property);
    }

    // Update existing property
    [RequireLogin]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProperty(Guid id, Property updatedProperty)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var existingProperty = await _propertyRepository.GetByIdAsync(id);
        if (existingProperty == null)
            return NotFound("Property not found.");

        // Hosts can only update their own properties
        if (existingProperty.Owner != loggedInUserId)
            return Forbid();

        // Update only fields that are provided
        existingProperty.Title = updatedProperty.Title ?? existingProperty.Title;
        existingProperty.City = updatedProperty.City ?? existingProperty.City;
        existingProperty.State = updatedProperty.State ?? existingProperty.State;
        existingProperty.StreetAddress = updatedProperty.StreetAddress ?? existingProperty.StreetAddress;
        existingProperty.Bedrooms = updatedProperty.Bedrooms > 0 ? updatedProperty.Bedrooms : existingProperty.Bedrooms;
        existingProperty.Bathrooms = updatedProperty.Bathrooms > 0 ? updatedProperty.Bathrooms : existingProperty.Bathrooms;
        existingProperty.PricePerNight = updatedProperty.PricePerNight > 0 ? updatedProperty.PricePerNight : existingProperty.PricePerNight;
        existingProperty.SquareFeet = updatedProperty.SquareFeet > 0 ? updatedProperty.SquareFeet : existingProperty.SquareFeet;
        existingProperty.Photos = updatedProperty.Photos ?? existingProperty.Photos;

        await _propertyRepository.UpdateAsync(existingProperty);
        return NoContent();
    }

    // Delete property
    [RequireLogin]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProperty(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var property = await _propertyRepository.GetByIdAsync(id);
        if (property == null)
            return NotFound("Property not found.");

        // Hosts can only delete their own properties
        if (property.Owner != loggedInUserId)
            return Forbid();

        await _propertyRepository.DeleteAsync(id);
        return NoContent();
    }
}
