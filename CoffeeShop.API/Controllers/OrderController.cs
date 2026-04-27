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

    public OrderController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var products = await _context.Products
            .Where(p => request.ProductIds.Contains(p.Id))
            .ToListAsync();

        if (products.Count != request.ProductIds.Distinct().Count())
        {
            return BadRequest("Деякі товари не знайдені в каталозі.");
        }

        var outOfStockProducts = products.Where(p => p.StockQuantity <= 0).ToList();
        if (outOfStockProducts.Any())
        {
            var names = string.Join(", ", outOfStockProducts.Select(p => p.Name));
            return BadRequest($"На жаль, наступні товари закінчилися: {names}");
        }

        foreach (var product in products)
        {
            product.StockQuantity -= 1;
        }

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
            
        decimal totalAmount = CalculateDiscountedTotal(products, activePromo);
        
        decimal subDiscountPercent = await GetSubscriptionDiscountPercentageAsync(request.CustomerId);
        totalAmount -= totalAmount * (subDiscountPercent / 100m);

        decimal bulkDiscountPercent = GetBulkDiscountPercentage(products.Count, totalAmount);
        totalAmount -= totalAmount * (bulkDiscountPercent / 100m);

        var newOrder = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.Now,
            Status = OrderStatus.New,
            TotalAmount = totalAmount,
            Products = products
        };

        _context.Orders.Add(newOrder);
        await _context.SaveChangesAsync();

        return Ok(new { orderId = newOrder.Id, total = Math.Round(totalAmount, 2), message = "Замовлення успішно створено, склад оновлено." });
    }

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

    private decimal CalculateDiscountedTotal(List<Product> products, PromoAction? promo)
    {
        decimal total = 0;

        foreach (var product in products)
        {
            decimal itemPrice = product.Price;

            if (promo != null && promo.Type == PromoType.CategoryBased && product.CategoryId == promo.TargetCategoryId)
            {
                itemPrice -= itemPrice * (promo.DiscountPercentage / 100m);
            }

            total += itemPrice;
        }

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

    private async Task<decimal> GetSubscriptionDiscountPercentageAsync(int customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.Subscription) 
            .FirstOrDefaultAsync(c => c.Id == customerId);

        if (customer?.Subscription == null)
            return 0m;

        if (customer.Subscription.Type == SubscriptionType.BasicCoffeePass)
            return 10m;
            
        if (customer.Subscription.Type == SubscriptionType.PremiumRoasterClub)
            return 15m;

        return 0m;
    }

    private decimal GetBulkDiscountPercentage(int itemsCount, decimal currentTotal)
    {
        if (itemsCount >= 5)
            return 10m; 

        if (currentTotal >= 1000)
            return 7m; 

        if (itemsCount >= 3)
            return 5m; 

        return 0m;
    }
}

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public string? PromoCode { get; set; } 
}