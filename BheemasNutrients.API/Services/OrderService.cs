using BheemasNutrients.API.Data;
using BheemasNutrients.API.DTOs;
using BheemasNutrients.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BheemasNutrients.API.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context) => _context = context;

    public async Task<OrderResponse> CreateAsync(int userId, CreateOrderRequest request)
    {
        var menuItemIds = request.Items.Select(i => i.MenuItemId).ToList();
        var menuItems = await _context.MenuItems
            .Where(m => menuItemIds.Contains(m.Id) && m.IsAvailable)
            .ToDictionaryAsync(m => m.Id);

        if (menuItems.Count != menuItemIds.Count)
            throw new InvalidOperationException("One or more menu items are unavailable.");

        var order = new Order
        {
            UserId = userId,
            DeliveryAddress = request.DeliveryAddress,
            Items = request.Items.Select(i => new OrderItem
            {
                MenuItemId = i.MenuItemId,
                Quantity = i.Quantity,
                UnitPrice = menuItems[i.MenuItemId].Price
            }).ToList()
        };

        order.TotalAmount = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return MapToResponse(order, menuItems);
    }

    public async Task<List<OrderResponse>> GetUserOrdersAsync(int userId)
    {
        var orders = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return orders.Select(o => MapToResponse(o, null)).ToList();
    }

    public async Task<OrderResponse> UpdateStatusAsync(int orderId, string status)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.MenuItem)
            .FirstOrDefaultAsync(o => o.Id == orderId)
            ?? throw new KeyNotFoundException($"Order {orderId} not found.");

        order.Status = status;
        if (status == "Delivered") order.DeliveredAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToResponse(order, null);
    }

    private static OrderResponse MapToResponse(Order order, Dictionary<int, MenuItem>? menuItems)
    {
        var items = order.Items.Select(i => new OrderItemResponse(
            i.MenuItem?.Name ?? menuItems?[i.MenuItemId].Name ?? $"Item {i.MenuItemId}",
            i.Quantity,
            i.UnitPrice)).ToList();

        return new OrderResponse(order.Id, order.TotalAmount, order.Status,
            order.DeliveryAddress, order.OrderDate, items);
    }
}
