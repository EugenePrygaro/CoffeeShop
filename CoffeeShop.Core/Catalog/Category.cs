namespace CoffeeShop.Core.Catalog;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Список продуктів, що належать до цієї категорії
    public List<Product> Products { get; set; } = new();
}