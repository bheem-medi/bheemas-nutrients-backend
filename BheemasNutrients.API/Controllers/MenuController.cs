using BheemasNutrients.API.DTOs;
using BheemasNutrients.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BheemasNutrients.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService) => _menuService = menuService;

    [HttpGet]
    public async Task<ActionResult<List<MenuItemResponse>>> GetAll([FromQuery] string? category)
        => Ok(await _menuService.GetAllAsync(category));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MenuItemResponse>> GetById(int id)
        => Ok(await _menuService.GetByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MenuItemResponse>> Create(CreateMenuItemRequest request)
        => Ok(await _menuService.CreateAsync(request));

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<MenuItemResponse>> Update(int id, UpdateMenuItemRequest request)
        => Ok(await _menuService.UpdateAsync(id, request));

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        await _menuService.DeleteAsync(id);
        return NoContent();
    }
}
