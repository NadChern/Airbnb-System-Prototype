using System.ComponentModel.DataAnnotations;
namespace AirbnbREST.Models;

public class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();
        
    [Required(ErrorMessage = "Owner ID is required")]
    public Guid Owner { get; set; }
        
    public DateTime CreatedAt { get; set; }
        
    [Required(ErrorMessage = "Number of bedrooms is required")]
    [Range(1, 20, ErrorMessage = "Bedrooms must be between 1 and 20")]
    public int Bedrooms { get; set; }
        
    [Required(ErrorMessage = "Number of bathrooms is required")]
    [Range(1, 20, ErrorMessage = "Bathrooms must be between 1 and 20")]
    public int Bathrooms { get; set; }
        
    [Required(ErrorMessage = "Square footage is required")]
    [Range(100, 10000, ErrorMessage = "Square feet must be between 100 and 10,000")]
    public int SquareFeet { get; set; }
        
    [Required(ErrorMessage = "Price per night is required")]
    [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10,000")]
    public int PricePerNight { get; set; }
        
    [Required(ErrorMessage = "Property title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; }
        
    [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
    public string About { get; set; }
        
    [Required(ErrorMessage = "Street address is required")]
    public string StreetAddress { get; set; }
        
    [Required(ErrorMessage = "City is required")]
    public string City { get; set; }
        
    [Required(ErrorMessage = "State is required")]
    public string State { get; set; }
        
    [Required(ErrorMessage = "ZIP code is required")]
    public string ZipCode { get; set; }
        
    // Navigation for photos
    public List<PropertyPhoto>? Photos { get; set; } 
}
