using System.Security.Claims;
using BheemasNutrients.API.DTOs;
using BheemasNutrients.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BheemasNutrients.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService) => _orderService = orderService;

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _orderService.CreateAsync(userId, request));
    }

    [HttpGet("my-orders")]
    public async Task<ActionResult<List<OrderResponse>>> GetMyOrders()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        return Ok(await _orderService.GetUserOrdersAsync(userId));
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Admin,Delivery")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(int id, [FromBody] string status)
        => Ok(await _orderService.UpdateStatusAsync(id, status));
}
