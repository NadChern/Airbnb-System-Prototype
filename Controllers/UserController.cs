using AirbnbREST.Models;
using AirbnbREST.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace AirbnbREST.Controllers;

// Delete request is not provided (to discuss)
// User should update booking status

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Get all users
    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userRepository.GetAllAsync();
    }

    // Get user by ID
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }

    // Create a new user
    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetUserById), 
            new { id = user.Id }, user);
    }

    // Update user details 
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(Guid id, User updatedUser)
    {
        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return NotFound("User not found.");

        // Update only fields that are provided
        existingUser.FirstName = updatedUser.FirstName ?? existingUser.FirstName;
        existingUser.LastName = updatedUser.LastName ?? existingUser.LastName;
        existingUser.Phone = updatedUser.Phone ?? existingUser.Phone;
        existingUser.Email = updatedUser.Email ?? existingUser.Email;
        existingUser.ProfilePicLink = updatedUser.ProfilePicLink ?? existingUser.ProfilePicLink;
        existingUser.Bio = updatedUser.Bio ?? existingUser.Bio;
        existingUser.Role = updatedUser.Role;

        await _userRepository.UpdateAsync(existingUser);
        return NoContent();
    }

    // Get user role by ID
    [HttpGet("{id}/role")]
    public async Task<ActionResult<UserRole>> GetUserRole(Guid id)
    {
        var role = await _userRepository.GetUserRoleAsync(id);
        if (role == null)
            return NotFound("User not found or role not assigned.");

        return Ok(role);
    }

    // Find user by name
    [HttpGet("search")]
    public async Task<IEnumerable<User>> FindUserByName([FromQuery] string firstName, 
        [FromQuery] string lastName)
    {
        return await _userRepository.FindByNameAsync(firstName, lastName);
    }

    // Get user by email
    [HttpGet("email/{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }
}
