using BheemasNutrients.API.DTOs;
using BheemasNutrients.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BheemasNutrients.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            return Ok(await _authService.RegisterAsync(request));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            return Ok(await _authService.LoginAsync(request));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }
}
