using CoffeeShop.Core.Entities;
using CoffeeShop.Infrastructure;


public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    // знижка по підписці виконана одним запитом до бази даних замість двох
    public decimal GetSubscriptionDiscount(int customerId)
    {
        return _context.Customers
            .Where(c => c.Id == customerId)
            .Select(c => c.Subscription != null ? c.Subscription.DiscountPercentage : 0)
            .FirstOrDefault();
    }

    // bulk знижка
    public decimal GetBulkDiscount(List<Product> products)
    {
        int count = products.Count;
        decimal total = products.Sum(p => p.Price);

        // правило по кількості      ???тут треба вирішити яка логіка знижок
        if (count >= 5)
            return 10;

        if (count >= 3)
            return 5;

        // правило по сумі
        if (total >= 1000)
            return 7;

        return 0;
    }

    // фінальний розрахунок
    public decimal CalculateTotal(int customerId, List<Product> products, decimal total)
    {
        var subscriptionDiscount = GetSubscriptionDiscount(customerId);
        var bulkDiscount = GetBulkDiscount(products);

        // Обираємо суму зі знижкою по підписці
        var total_with_subscribe = total * (1 - subscriptionDiscount / 100);

        // Обраховуємо загальну суму зі знижкою по підписці і кількості
        var total_with_subscribe_and_bulk = total_with_subscribe * (1 - bulkDiscount / 100) ;
        
       
        // Обраховуємо загальну суму з знижкою
        return total_with_subscribe_and_bulk;
       
    }
}