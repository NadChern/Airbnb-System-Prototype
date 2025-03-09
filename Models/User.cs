using System.ComponentModel.DataAnnotations;
namespace AirbnbREST.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    
    public string? MiddleName { get; set; }
    
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
        
    public string? ProfilePicLink { get; set; }
    
    public string? Bio { get; set; }
        
    [Required(ErrorMessage = "User role is required")]
    public UserRole Role { get; set; }
}
