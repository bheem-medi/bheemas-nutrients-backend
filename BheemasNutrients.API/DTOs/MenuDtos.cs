namespace BheemasNutrients.API.DTOs;

public record CreateMenuItemRequest(string Name, string Description, decimal Price, string Category, int Calories, string ImageUrl);
public record UpdateMenuItemRequest(string Name, string Description, decimal Price, string Category, bool IsAvailable, int Calories, string ImageUrl);
public record MenuItemResponse(int Id, string Name, string Description, decimal Price, string Category, bool IsAvailable, int Calories, string ImageUrl);
