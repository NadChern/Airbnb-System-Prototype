using AirbnbREST.DTOs;
using AirbnbREST.Middleware;
using AirbnbREST.Models;
using AirbnbREST.Repositories;
using AirbnbREST.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AirbnbREST.Controllers;

// Delete request is not provided (to discuss with team,
// requires additional logic like user cannot be deleted with active booking etc)

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Get user by ID (only self-access)
    // [RequireLogin]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(Guid id)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

        // Users can only access their own profile
        if (loggedInUserId != id)
            return Forbid(); // 403 Forbidden

        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }

    // Register new user
    [HttpPost("register")]
    public async Task<ActionResult<User>> RegisterUser(User user)
    {
        // Check if the email is already in use
        var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        if (existingUser != null)
            return BadRequest("Email is already registered.");

        // Hash the user's password before saving
        user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);

        await _userRepository.AddAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    // Login a user
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null)
            return Unauthorized("Invalid email or password.");

        // Verify password
        if (!PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        // Store user session (sessions enabled in Program + stored in memory
        // using distributed memory cache)
        HttpContext.Session.SetString("UserId", user.Id.ToString());

        return Ok("Login successful.");
    }


    // Update user details (only self-access)
    // [RequireLogin]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(Guid id, UserUpdateDto updateDto)
    {
        var loggedInUserId = Guid.Parse(HttpContext.Session.GetString("UserId"));

        // Users can only update their own profile
        if (loggedInUserId != id)
            return Forbid();

        var existingUser = await _userRepository.GetByIdAsync(id);
        if (existingUser == null)
            return NotFound("User not found.");

        // Update only fields that are provided
        if (updateDto.FirstName != null) existingUser.FirstName = updateDto.FirstName;
        if (updateDto.MiddleName != null) existingUser.MiddleName = updateDto.MiddleName;
        if (updateDto.LastName != null) existingUser.LastName = updateDto.LastName;
        if (updateDto.Phone != null) existingUser.Phone = updateDto.Phone;
        if (updateDto.Email != null) existingUser.Email = updateDto.Email;
        if (updateDto.ProfilePicLink != null) existingUser.ProfilePicLink = updateDto.ProfilePicLink;
        if (updateDto.Bio != null) existingUser.Bio = updateDto.Bio;

        await _userRepository.UpdateAsync(existingUser);
        return NoContent();
    }

    // Logout (session timeout is set in Program.cs
    // If the user is inactive for 30 minutes, the session automatically expires.
    // When the user tries to make a request, they will not be logged in anymore.
    // They will get 401 Unauthorized and need to log in again.
    [RequireLogin]
    [HttpPost("logout")]
    public ActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear session data
        return Ok("Logged out successfully."); // User’s session ID is deleted on the server.
    }


    // Get user role by ID
    [RequireLogin]
    [HttpGet("{id}/role")]
    public async Task<ActionResult<UserRole>> GetUserRole(Guid id)
    {
        var role = await _userRepository.GetUserRoleAsync(id);
        if (role == null)
            return NotFound("User not found or role not assigned.");

        return Ok(role);
    }

    // REST FUNC FOR TESTING ONLY. DELETE FOR SUBMITTION
    // Get all users (for testing purpose only - delete on submission!)
    [HttpGet]
    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userRepository.GetAllAsync();
    }
}
