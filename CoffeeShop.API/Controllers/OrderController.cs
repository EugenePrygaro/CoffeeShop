using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/order
    // Метод приймає ID клієнта та масив вибраних продуктів з кошика
    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
    {
        // 1. Шукаємо продукти, які вибрав користувач
        var products = _context.Products
            .Where(p => request.ProductIds.Contains(p.Id))
            .ToList();

        // Перевірка: чи всі ID, що прийшли з фронту, існують у базі
        if (products.Count != request.ProductIds.Distinct().Count())
        {
            return BadRequest("Деякі товари не знайдені в каталозі.");
        }

        // 2. ПЕРЕВІРКА НА СКЛАДІ
        // Шукаємо товари, кількість яких дорівнює 0
        var outOfStockProducts = products.Where(p => p.StockQuantity <= 0).ToList();

        if (outOfStockProducts.Any())
        {
            var names = string.Join(", ", outOfStockProducts.Select(p => p.Name));
            return BadRequest($"На жаль, наступні товари закінчилися: {names}");
        }

        // 3. ЗМЕНШЕННЯ КІЛЬКОСТІ НА СКЛАДІ
        // Проходимо по кожному товару в замовленні та віднімаємо одиницю
        foreach (var product in products)
        {
            product.StockQuantity -= 1;
        }

        // 4. Формуємо замовлення
        var newOrder = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.New,
            TotalAmount = products.Sum(p => p.Price),
            Products = products
        };

        // 5. Зберігаємо зміни
        // Entity Framework оновить і таблицю Orders, і поле StockQuantity у таблиці Products в одній транзакції
        _context.Orders.Add(newOrder);
        _context.SaveChanges();

        return Ok(new { orderId = newOrder.Id, message = "Замовлення успішно створено, склад оновлено." });
    }

    // GET: api/order/{id}
    // Метод повертає виключно статус конкретного замовлення
    [HttpGet("{id}")]
    public IActionResult GetOrderStatus(int id)
    {
        // Використовуємо .Select(), щоб сформувати точковий запит.
        // Завдяки цьому MySQL навіть не буде читати колонки TotalAmount чи OrderDate,
        // і тим більше не буде підтягувати пов'язані продукти. Ми беремо тільки Status.
        var orderStatus = _context.Orders
            .Where(o => o.Id == id)
            .Select(o => new { status = o.Status.ToString() }) // Перетворюємо Enum (0, 1, 2) на текст ("New", "Paid")
            .FirstOrDefault();

        if (orderStatus == null)
        {
            return NotFound("Замовлення не знайдено.");
        }

        // Віддаємо на фронт акуратний JSON: { "status": "New" }
        return Ok(orderStatus);
    }
}

// Допоміжний клас (DTO) для прийому даних від фронтенду.
// Розміщений тут же для зручності.
public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<int> ProductIds { get; set; } = new();
}