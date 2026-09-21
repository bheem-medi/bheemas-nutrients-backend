using BheemasNutrients.API.Data;
using BheemasNutrients.API.DTOs;
using BheemasNutrients.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BheemasNutrients.API.Services;

public class MenuService : IMenuService
{
    private readonly AppDbContext _context;

    public MenuService(AppDbContext context) => _context = context;

    public async Task<List<MenuItemResponse>> GetAllAsync(string? category = null)
    {
        var query = _context.MenuItems.AsQueryable();
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(m => m.Category == category);

        return await query
            .Where(m => m.IsAvailable)
            .Select(m => MapToResponse(m))
            .ToListAsync();
    }

    public async Task<MenuItemResponse> GetByIdAsync(int id)
    {
        var item = await _context.MenuItems.FindAsync(id)
            ?? throw new KeyNotFoundException($"Menu item {id} not found.");
        return MapToResponse(item);
    }

    public async Task<MenuItemResponse> CreateAsync(CreateMenuItemRequest request)
    {
        var item = new MenuItem
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            Calories = request.Calories,
            ImageUrl = request.ImageUrl
        };

        _context.MenuItems.Add(item);
        await _context.SaveChangesAsync();
        return MapToResponse(item);
    }

    public async Task<MenuItemResponse> UpdateAsync(int id, UpdateMenuItemRequest request)
    {
        var item = await _context.MenuItems.FindAsync(id)
            ?? throw new KeyNotFoundException($"Menu item {id} not found.");

        item.Name = request.Name;
        item.Description = request.Description;
        item.Price = request.Price;
        item.Category = request.Category;
        item.IsAvailable = request.IsAvailable;
        item.Calories = request.Calories;
        item.ImageUrl = request.ImageUrl;

        await _context.SaveChangesAsync();
        return MapToResponse(item);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _context.MenuItems.FindAsync(id)
            ?? throw new KeyNotFoundException($"Menu item {id} not found.");
        _context.MenuItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    private static MenuItemResponse MapToResponse(MenuItem m) =>
        new(m.Id, m.Name, m.Description, m.Price, m.Category, m.IsAvailable, m.Calories, m.ImageUrl);
}
