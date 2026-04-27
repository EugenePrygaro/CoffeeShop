using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController : ControllerBase
{
    private readonly AppDbContext _context;

    public CustomerController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> GetAll()
    {
        return await _context.Customers
            .Include(c => c.Subscription)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Subscription)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null) return NotFound("Клієнта не знайдено.");
        return customer;
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> Create(Customer customer)
    {
        var exists = await _context.Customers.AnyAsync(c => c.Email == customer.Email);
        if (exists) return BadRequest("Клієнт із такою поштою вже існує.");

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateDto dto)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound("Клієнта для оновлення не знайдено.");

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.Email = dto.Email;

        await _context.SaveChangesAsync();
        return Ok(new { message = "Дані клієнта успішно оновлено." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound("Клієнта не знайдено.");

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Клієнта видалено з бази даних." });
    }

    [HttpGet("{id}/loyalty")]
    public async Task<IActionResult> GetLoyaltyStatus(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Subscription)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer == null) return NotFound("Клієнта не знайдено.");

        if (customer.Subscription != null)
        {
            return Ok(new 
            { 
                hasLoyalty = true, 
                subscriptionType = customer.Subscription.Type.ToString(),
            });
        }

        return Ok(new { hasLoyalty = false });
    }

    [HttpPut("{id}/loyalty")]
    public async Task<IActionResult> UpdateLoyaltyStatus(int id, [FromBody] int? subscriptionId)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound("Клієнта не знайдено.");

        customer.SubscriptionId = subscriptionId;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Статус лояльності клієнта успішно оновлено." });
    }
}

public class CustomerUpdateDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}