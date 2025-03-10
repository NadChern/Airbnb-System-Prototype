namespace AirbnbREST.DTOs;

// we don't want to expose all user details during login -> use DTO
public class LoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}
