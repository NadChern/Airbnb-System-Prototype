using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace AirbnbREST.Models;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedAt { get; set; }
   
    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; }
    
    public string? MiddleName { get; set; }
    
    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? Phone { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    // Password Hash (prevent to store raw passwords)
    [Required(ErrorMessage = "Password is required")]
    public string PasswordHash { get; set; }
        
    [Url(ErrorMessage = "Invalid profile picture URL format")]
    public string? ProfilePicLink { get; set; }
    
    public string? Bio { get; set; }
    
    [Required(ErrorMessage = "User role is required")]   
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public UserRole Role { get; set; }
}
