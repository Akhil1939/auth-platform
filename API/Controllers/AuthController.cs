using API.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("api/{tenantSlug}/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(string tenantSlug, [FromBody] RegisterRequest request)
    {
        try
        {
            await _authService.RegisterUserAsync(tenantSlug, request.Email, request.Password);
            return Ok(new { message = "User registered" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string tenantSlug, [FromBody] LoginRequest request)
    {
        try
        {
            var token = await _authService.LoginAsync(tenantSlug, request.Email, request.Password);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
}
