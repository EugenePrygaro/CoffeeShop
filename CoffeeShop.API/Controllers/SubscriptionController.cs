using Microsoft.AspNetCore.Mvc;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

public interface ISubscriptionController
{
    IActionResult GetAllSubscriptions();
    IActionResult AssignSubscription([FromBody] AssignSubscriptionRequest request);
}

[ApiController]
[Route("api/[controller]")]
public class SubscriptionController : ControllerBase, ISubscriptionController
{
    private readonly AppDbContext _context;

    public SubscriptionController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetAllSubscriptions()
    {
        var subscriptions = _context.Subscriptions
            .Select(s => new 
            { 
                s.Id, 
                Type = s.Type.ToString(),
                s.Price, 
                s.DurationInDays 
            })
            .ToList();

        return Ok(subscriptions);
    }

    [HttpPost("assign")]
    public IActionResult AssignSubscription([FromBody] AssignSubscriptionRequest request)
    {
        var customer = _context.Customers.FirstOrDefault(c => c.Id == request.CustomerId);
        if (customer == null) return NotFound("Клієнта не знайдено.");

        // Якщо передали ID = 0, значить ми хочемо скасувати підписку
        if (request.SubscriptionId == 0)
        {
            customer.SubscriptionId = null; // Встановлюємо технічний null у базу
            _context.SaveChanges();
            return Ok(new { message = "Підписку успішно скасовано." });
        }

        // В іншому випадку шукаємо підписку в таблиці
        var subscription = _context.Subscriptions.FirstOrDefault(s => s.Id == request.SubscriptionId);
        if (subscription == null) return NotFound("Підписку не знайдено.");

        customer.SubscriptionId = subscription.Id;
        _context.SaveChanges();

        return Ok(new { message = $"Підписку {subscription.Type} оформлено." });
    }
}

public class AssignSubscriptionRequest
{
    public int CustomerId { get; set; }
    public int SubscriptionId { get; set; }
}