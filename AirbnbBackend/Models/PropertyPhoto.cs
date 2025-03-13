using System.ComponentModel.DataAnnotations;

namespace AirbnbREST.Models;

public class PropertyPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required(ErrorMessage = "Property ID is required")]
    public Guid PropertyId { get; set; } // Foreign key to Property
    
    [Required(ErrorMessage = "Photo URL is required")]
    [Url(ErrorMessage = "Invalid photo URL format")]
    public string PhotoUrl { get; set; } // URL of the property
    
}
