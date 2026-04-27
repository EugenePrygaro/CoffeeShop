using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;

namespace CoffeeShop.API.Controllers;

[ApiController]
[Route("api/promo")]
public class PromoController : ControllerBase
{
    private readonly AppDbContext _context;

    public PromoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("validate")]
    public async Task<IActionResult> ValidatePromo([FromBody] ValidatePromoRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Code))
        {
            return BadRequest(new { isValid = false, message = "Промокод не може бути порожнім." });
        }

        var promo = await _context.PromoActions
            .FirstOrDefaultAsync(p => p.PromoCode == request.Code.ToUpper() && p.IsActive);

        if (promo == null)
        {
            return NotFound(new { isValid = false, message = "Такого промокоду не існує або він неактивний." });
        }

        if (promo.Type == PromoType.TimeBased)
        {
            if (promo.ExpiryDate.HasValue && promo.ExpiryDate.Value < DateTime.Now)
            {
                return BadRequest(new { isValid = false, message = "Термін дії цього промокоду вже минув." });
            }

            return Ok(new 
            { 
                isValid = true, 
                type = "TimeBased",
                discountPercentage = promo.DiscountPercentage,
                expiryDate = promo.ExpiryDate,
                message = $"Знижка {promo.DiscountPercentage}% на все замовлення."
            });
        }

        if (promo.Type == PromoType.CategoryBased)
        {
            var category = await _context.Categories.FindAsync(promo.TargetCategoryId);
            var categoryName = category != null ? category.Name : "Невідома категорія";

            return Ok(new 
            { 
                isValid = true, 
                type = "CategoryBased",
                discountPercentage = promo.DiscountPercentage,
                targetCategory = categoryName,
                message = $"Знижка {promo.DiscountPercentage}% на категорію {categoryName}."
            });
        }

        return BadRequest(new { isValid = false, message = "Невідомий тип промокоду." });
    }
}

public class ValidatePromoRequest
{
    public string Code { get; set; } = string.Empty;
}