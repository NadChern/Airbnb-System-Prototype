using System.ComponentModel.DataAnnotations;
namespace AirbnbREST.Models;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();
        
    [Required(ErrorMessage = "Property ID is required")]
    public Guid PropertyId { get; set; }
        
    [Required(ErrorMessage = "Guest ID is required")]
    public Guid GuestId { get; set; }
        
    public DateTime UpdatedAt { get; set; }
        
    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }
        
    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }
    
    // Every new booking is always created as requested to prevent status errors
    [Required(ErrorMessage = "Booking status is required")]
    public BookingStatus Status { get; set; } = BookingStatus.Requested;
}
