using BheemasNutrients.API.DTOs;

namespace BheemasNutrients.API.Services;

public interface IOrderService
{
    Task<OrderResponse> CreateAsync(int userId, CreateOrderRequest request);
    Task<List<OrderResponse>> GetUserOrdersAsync(int userId);
    Task<OrderResponse> UpdateStatusAsync(int orderId, string status);
}
