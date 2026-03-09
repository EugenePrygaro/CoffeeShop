namespace CoffeeShop.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; } // Кількість на складі
    public int RoastLevel { get; set; }     // Рівень обсмаження (1-5)
    public double Weight { get; set; }      // Вага в грамах або кг

    // Зв'язок із категорією
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    
    // Зв'язок із замовленнями (Many-to-Many)
    public List<Order> Orders { get; set; } = new();
}