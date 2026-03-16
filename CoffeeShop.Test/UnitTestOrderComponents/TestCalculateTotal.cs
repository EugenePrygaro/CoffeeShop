using Microsoft.EntityFrameworkCore;
using CoffeeShop.Infrastructure;
using CoffeeShop.Core.Entities;


namespace CoffeeShop.Tests;

public class OrderServiceTests
{
    private AppDbContext GetContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        // додаємо підписку
        var subscription = new Subscription
        {
            Id = 1,
            DiscountPercentage = 10
        };

        context.Subscriptions.Add(subscription);

        // клієнт з підпискою
        context.Customers.Add(new Customer
        {
            Id = 1,
            SubscriptionId = 1
        });

        // клієнт без підписки
        context.Customers.Add(new Customer
        {
            Id = 2,
            SubscriptionId = null
        });

        context.SaveChanges();

        return context;
    }

    // ---------- GetSubscriptionDiscount ----------

    [Fact]
    public void GetSubscriptionDiscount_WithSubscription_ReturnsDiscount()
    {
        var context = GetContext();
        var service = new OrderService(context);

        var result = service.GetSubscriptionDiscount(1);

        Assert.Equal(10, result);
    }

    [Fact]
    public void GetSubscriptionDiscount_NoSubscription_ReturnsZero()
    {
        var context = GetContext();
        var service = new OrderService(context);

        var result = service.GetSubscriptionDiscount(2);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetSubscriptionDiscount_CustomerNotFound_ReturnsZero()
    {
        var context = GetContext();
        var service = new OrderService(context);

        var result = service.GetSubscriptionDiscount(999);

        Assert.Equal(0, result);
    }

    // ---------- GetBulkDiscount ----------

    [Fact]
    public void GetBulkDiscount_5Products_Returns10Percent()
    {
        var service = new OrderService(null);

        var products = new List<Product>
        {
            new Product { Price = 100 },
            new Product { Price = 100 },
            new Product { Price = 100 },
            new Product { Price = 100 },
            new Product { Price = 100 }
        };

        var result = service.GetBulkDiscount(products);

        Assert.Equal(10, result);
    }

    [Fact]
    public void GetBulkDiscount_3Products_Returns5Percent()
    {
        var service = new OrderService(null);

        var products = new List<Product>
        {
            new Product { Price = 200 },
            new Product { Price = 200 },
            new Product { Price = 200 }
        };

        var result = service.GetBulkDiscount(products);

        Assert.Equal(5, result);
    }

    [Fact]
    public void GetBulkDiscount_TotalOver1000_Returns7Percent()
    {
        var service = new OrderService(null);

        var products = new List<Product>
        {
            new Product { Price = 600 },
            new Product { Price = 500 }
        };

        var result = service.GetBulkDiscount(products);

        Assert.Equal(7, result);
    }

    [Fact]
    public void GetBulkDiscount_NoDiscount_ReturnsZero()
    {
        var service = new OrderService(null);

        var products = new List<Product>
        {
            new Product { Price = 100 },
            new Product { Price = 200 }
        };

        var result = service.GetBulkDiscount(products);

        Assert.Equal(0, result);
    }

    // ---------- CalculateTotal ----------

    [Fact]
    public void CalculateTotal_SubscriptionAndBulkDiscount_ReturnsCorrectTotal()
    {
        var context = GetContext();
        var service = new OrderService(context);

        var products = new List<Product>
        {
            new Product { Price = 300 },
            new Product { Price = 300 },
            new Product { Price = 300 }
        };

        decimal total = products.Sum(p => p.Price); // 900

        var result = service.CalculateTotal(1, products, total);

        // 900 -10% = 810
        // 810 -5% = 769.5
        Assert.Equal(769.5m, result);
    }

    [Fact]
    public void CalculateTotal_NoDiscount_ReturnsOriginalTotal()
    {
        var context = GetContext();
        var service = new OrderService(context);

        var products = new List<Product>
        {
            new Product { Price = 100 },
            new Product { Price = 200 }
        };

        decimal total = 300;

        var result = service.CalculateTotal(2, products, total);

        Assert.Equal(300, result);
    }
}