using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Sales;
using CoffeeShop.Core.Catalog;
using CoffeeShop.Core.Loyalty;
using CoffeeShop.Core.ValueObjects;

namespace CoffeeShop.API.UseCases; // Або CoffeeShop.API.UseCases

// 1. Команда (Command) - це просто DTO з даними для створення
public record CreateOrderCommand(int CustomerId, List<int> ProductIds, string? PromoCode);

// 2. Обробник команди (Command Handler) - оркеструє процес
public class CreateOrderCommandHandler
{
    private readonly AppDbContext _context;

    public CreateOrderCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateOrderCommand command)
    {
        // 1. Завантаження необхідних даних
        var products = await _context.Products
            .Where(p => command.ProductIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != command.ProductIds.Distinct().Count())
            throw new Exception("Деякі товари не знайдені в каталозі.");

        var outOfStockProducts = products.Where(p => p.StockQuantity <= 0).ToList();
        if (outOfStockProducts.Any())
            throw new Exception($"Товари закінчилися: {string.Join(", ", outOfStockProducts.Select(p => p.Name))}");

        // 2. Створення агрегату через фабричний метод
        var order = Order.Create(command.CustomerId);

        // 3. Виклик бізнес-логіки агрегату (додавання продуктів та списання зі складу)
        foreach (var product in products)
        {
            product.StockQuantity -= 1;
            order.AddProduct(product); 
        }

        // --- Блок розрахунку знижок (спрощено для прикладу) ---
        // Оскільки наша мета - показати роботу агрегату, знижки ми рахуємо 
        // і в майбутньому можемо додати метод order.ApplyDiscount() в Агрегат.
        
        // 4. Збереження агрегату. 
        // Саме тут спрацює наш DbContext.SaveChangesAsync, який
        // автоматично "зловить" подію OrderCreatedEvent і виведе її в консоль!
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order.Id;
    }
}