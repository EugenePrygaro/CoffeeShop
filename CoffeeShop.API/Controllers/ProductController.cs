using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/product")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }

    // GET: api/product/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Товар не знайдено.");
        }

        return product;
    }

    // POST: api/product
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/product/{id}
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] ProductUpdateDto updateDto)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound("Товар не знайдено.");
        }

        // Оновлюємо тільки ціну та опис, як вказано в техзавданні
        product.Price = updateDto.Price;
        product.Description = updateDto.Description;

        _context.SaveChanges();
        return Ok(new { message = "Інформацію про товар оновлено." });
    }

    // DELETE: api/product/{id}
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound("Товар не знайдено.");
        }

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(new { message = $"Товар {product.Name} видалено з асортименту." });
    }
}

// Допоміжна модель для оновлення (DTO)
public class ProductUpdateDto
{
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}