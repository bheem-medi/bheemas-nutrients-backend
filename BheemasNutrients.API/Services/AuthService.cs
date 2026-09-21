using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BheemasNutrients.API.Data;
using BheemasNutrients.API.DTOs;
using BheemasNutrients.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BheemasNutrients.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException("Email already registered.");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            PhoneNumber = request.PhoneNumber
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return GenerateToken(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new UnauthorizedAccessException("Invalid credentials.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid credentials.");

        return GenerateToken(user);
    }

    private AuthResponse GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not configured")));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials);

        return new AuthResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            user.FullName,
            user.Email,
            user.Role);
    }
}
