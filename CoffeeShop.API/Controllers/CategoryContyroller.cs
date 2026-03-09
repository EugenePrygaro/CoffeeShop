using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/category")]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/category
    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var categories = _context.Categories
            .Include(c => c.Products) // 1. Завантажуємо пов'язані продукти з бази
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                // 2. Формуємо масив назв продуктів
                ProductNames = c.Products.Select(p => p.Name).ToList(),
                // 3. Формуємо рядок назв через кому
                ProductsSummary = string.Join(", ", c.Products.Select(p => p.Name))
            })
            .ToList();

        return Ok(categories);
    }

    // GET: api/category/1
    [HttpGet("{id}")]
    public IActionResult GetCategoryById(int id)
    {
        var category = _context.Categories
            .Include(c => c.Products)
            .Where(c => c.Id == id)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                ProductNames = c.Products.Select(p => p.Name).ToList(),
                ProductsSummary = string.Join(", ", c.Products.Select(p => p.Name))
            })
            .FirstOrDefault();

        if (category == null)
        {
            return NotFound("Категорію не знайдено.");
        }

        return Ok(category);
    }
}