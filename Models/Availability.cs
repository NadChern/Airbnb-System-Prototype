using System.ComponentModel.DataAnnotations;
namespace AirbnbREST.Models;

public class Availability
{
    public Guid Id { get; set; } = Guid.NewGuid();
        
    [Required(ErrorMessage = "Property ID is required")]
    public Guid PropertyId { get; set; }
        
    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }
        
    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }
}
