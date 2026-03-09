namespace CoffeeShop.Core.Entities;

public enum OrderStatus
{
    New,        // 0 - Нове
    Paid,       // 1 - Оплачене
    Delivered   // 2 - Доставлене
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public int CustomerId { get; set; }
    
    // Масив продуктів, які належать до цього замовлення
    public List<Product> Products { get; set; } = new();
}