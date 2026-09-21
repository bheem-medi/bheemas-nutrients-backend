namespace BheemasNutrients.API.DTOs;

public record CreateOrderRequest(string DeliveryAddress, List<OrderItemRequest> Items);
public record OrderItemRequest(int MenuItemId, int Quantity);
public record OrderResponse(int Id, decimal TotalAmount, string Status, string DeliveryAddress, DateTime OrderDate, List<OrderItemResponse> Items);
public record OrderItemResponse(string Name, int Quantity, decimal UnitPrice);
