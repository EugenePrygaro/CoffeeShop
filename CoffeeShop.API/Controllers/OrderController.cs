using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly OrderService _orderService;

    public OrderController(AppDbContext context, OrderService orderService)
    {
        _context = context;
        _orderService = orderService;
    }

    // POST: api/order
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        // 1. Шукаємо продукти, які вибрав користувач (асинхронно)
        var products = await _context.Products
            .Where(p => request.ProductIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != request.ProductIds.Distinct().Count())
        {
            return BadRequest("Деякі товари не знайдені в каталозі.");
        }

        // 2. ПЕРЕВІРКА НА СКЛАДІ
        var outOfStockProducts = products.Where(p => p.StockQuantity <= 0).ToList();
        if (outOfStockProducts.Any())
        {
            var names = string.Join(", ", outOfStockProducts.Select(p => p.Name));
            return BadRequest($"На жаль, наступні товари закінчилися: {names}");
        }

        // 3. ЗМЕНШЕННЯ КІЛЬКОСТІ НА СКЛАДІ
        foreach (var product in products)
        {
            product.StockQuantity -= 1;
        }

        // 4. ПЕРЕВІРКА ПРОМОКОДУ
        PromoAction? activePromo = null;
        if (!string.IsNullOrWhiteSpace(request.PromoCode))
        {
            activePromo = await _context.PromoActions
                .FirstOrDefaultAsync(p => p.PromoCode == request.PromoCode.ToUpper() && p.IsActive);
                
            if (activePromo == null)
            {
                return BadRequest("Недійсний або неактивний промокод.");
            }
        }

        // 5. ПІДРАХУНОК ФІНАЛЬНОЇ ВАРТОСТІ (через ізольований метод)
        decimal totalAmount = CalculateDiscountedTotal(products, activePromo);

        totalAmount = _orderService.CalculateTotal(request.CustomerId, products, totalAmount);

        // 6. Формуємо замовлення
        var newOrder = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.New,
            TotalAmount = totalAmount,
            Products = products
        };

        // 7. Зберігаємо зміни (асинхронно)
        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        return Ok(new { orderId = newOrder.Id, total = totalAmount, message = "Замовлення успішно створено, склад оновлено." });
    }

    // GET: api/order/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        var orderStatus = await _context.Orders
            .Where(o => o.Id == id)
            .Select(o => new { status = o.Status.ToString() }) 
            .FirstOrDefaultAsync();

        if (orderStatus == null)
        {
            return NotFound("Замовлення не знайдено.");
        }

        return Ok(orderStatus);
    }
    
    // PUT: api/order/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound("Замовлення не знайдено.");

        try
        {
            order.ChangeStatus(newStatus);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Статус замовлення успішно оновлено на {newStatus}" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // --- ДОПОМІЖНІ МЕТОДИ ТА КЛАСИ ---

    private decimal CalculateDiscountedTotal(List<Product> products, PromoAction? promo)
    {
        decimal total = 0;

        foreach (var product in products)
        {
            decimal itemPrice = product.Price;

            // Категорійна знижка (на конкретні товари)
            if (promo != null && promo.Type == PromoType.CategoryBased && product.CategoryId == promo.TargetCategoryId)
            {
                itemPrice -= itemPrice * (promo.DiscountPercentage / 100m);
            }

            total += itemPrice;
        }

        // Часова знижка (на весь чек)
        if (promo != null && promo.Type == PromoType.TimeBased)
        {
            if (promo.ExpiryDate.HasValue && promo.ExpiryDate.Value >= DateTime.Now)
            {
                total -= total * (promo.DiscountPercentage / 100m);
            }
            else
            {
                throw new InvalidOperationException("Термін дії цього промокоду вже минув.");
            }
        }

        return total;
    }
}

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public string? PromoCode { get; set; } // Додано поле для промокоду
}