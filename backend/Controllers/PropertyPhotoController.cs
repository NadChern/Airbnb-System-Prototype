using AirbnbREST.Middleware;
using AirbnbREST.Models;
using AirbnbREST.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AirbnbREST.Controllers;

[ApiController]
[Route("api/property-photos")]
public class PropertyPhotoController : ControllerBase
{
    private readonly IPropertyPhotoRepository _photoRepository;
    private readonly IPropertyRepository _propertyRepository;

    public PropertyPhotoController(
        IPropertyPhotoRepository photoRepository, 
        IPropertyRepository propertyRepository, 
        IUserRepository userRepository)
    {
        _photoRepository = photoRepository;
        _propertyRepository = propertyRepository;
 }

    // Get all photos for a property
    [HttpGet("{propertyId}")]
    public async Task<IEnumerable<PropertyPhoto>> GetPropertyPhotos(Guid propertyId)
    {
        return await _photoRepository.GetPhotosByPropertyIdAsync(propertyId);
    }

    // Add a new property photo (only hosts can add)
    [RequireLogin]
    [HttpPost]
    public async Task<ActionResult<PropertyPhoto>> AddPhoto(PropertyPhoto photo)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var property = await _propertyRepository.GetByIdAsync(photo.PropertyId);

        if (property == null)
            return BadRequest("Property does not exist.");

        if (property.Owner != loggedInUserId)
            return Forbid(); // Only the host can add photos

        await _photoRepository.AddAsync(photo);
        return CreatedAtAction(nameof(GetPropertyPhotos), new { propertyId = photo.PropertyId }, photo);
    }

    // Delete a property photo (only hosts can delete)
    [RequireLogin]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePhoto(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        var photo = await _photoRepository.GetByIdAsync(id);

        if (photo == null)
            return NotFound("Photo not found.");

        var property = await _propertyRepository.GetByIdAsync(photo.PropertyId);

        if (property == null || property.Owner != loggedInUserId)
            return Forbid(); // Only the host can delete photos

        await _photoRepository.DeleteAsync(id);
        return NoContent();
    }
}
