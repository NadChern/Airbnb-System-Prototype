using System.ComponentModel.DataAnnotations;

namespace AirbnbREST.Models;

public class PropertyPhoto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    public Guid PropertyId { get; set; } // Foreign key to Property
    
    [Required]
    public string PhotoUrl { get; set; } // URL or storage path of the photo
    
}
