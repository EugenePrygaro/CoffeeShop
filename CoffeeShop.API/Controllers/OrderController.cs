using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Sales;
using CoffeeShop.API.UseCases; // Підключаємо наш Handler

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly CreateOrderCommandHandler _handler;

    // Інжектимо наш Handler у контролер
    public OrderController(AppDbContext context, CreateOrderCommandHandler handler)
    {
        _context = context;
        _handler = handler;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        try
        {
            // Формуємо команду і передаємо в Handler
            var command = new CreateOrderCommand(request.CustomerId, request.ProductIds, request.PromoCode);
            var orderId = await _handler.Handle(command);

            return Ok(new { orderId = orderId, message = "Замовлення успішно створено, склад оновлено." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderStatus(int id)
    {
        var orderStatus = await _context.Orders
            .Where(o => o.Id == id)
            .Select(o => new { status = o.Status.ToString() }) 
            .FirstOrDefaultAsync();

        if (orderStatus == null) return NotFound("Замовлення не знайдено.");
        return Ok(orderStatus);
    }
    
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatus newStatus)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound("Замовлення не знайдено.");

        try
        {
            // Виклик бізнес-методу агрегату
            order.ChangeStatus(newStatus);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Статус замовлення успішно оновлено на {newStatus}" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public List<int> ProductIds { get; set; } = new();
    public string? PromoCode { get; set; } 
}