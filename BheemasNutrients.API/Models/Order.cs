namespace BheemasNutrients.API.Models;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public string DeliveryAddress { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public DateTime? DeliveredAt { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
