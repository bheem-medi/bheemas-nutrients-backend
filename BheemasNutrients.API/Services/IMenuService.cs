using BheemasNutrients.API.DTOs;

namespace BheemasNutrients.API.Services;

public interface IMenuService
{
    Task<List<MenuItemResponse>> GetAllAsync(string? category = null);
    Task<MenuItemResponse> GetByIdAsync(int id);
    Task<MenuItemResponse> CreateAsync(CreateMenuItemRequest request);
    Task<MenuItemResponse> UpdateAsync(int id, UpdateMenuItemRequest request);
    Task DeleteAsync(int id);
}
