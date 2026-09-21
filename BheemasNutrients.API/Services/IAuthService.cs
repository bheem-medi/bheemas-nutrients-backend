using BheemasNutrients.API.DTOs;

namespace BheemasNutrients.API.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
}
