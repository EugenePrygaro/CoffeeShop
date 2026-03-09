using CoffeeShop.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoffeeShop.Infrastructure;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
        
        context.Database.ExecuteSqlRaw("DELETE FROM OrderProduct;"); 
        context.Database.ExecuteSqlRaw("DELETE FROM Orders;");
        context.Database.ExecuteSqlRaw("DELETE FROM Customers;");
        context.Database.ExecuteSqlRaw("DELETE FROM Subscriptions;");
        context.Database.ExecuteSqlRaw("DELETE FROM Products;");
        context.Database.ExecuteSqlRaw("DELETE FROM Categories;");
        
        context.Database.ExecuteSqlRaw("ALTER TABLE Orders AUTO_INCREMENT = 1;");
        context.Database.ExecuteSqlRaw("ALTER TABLE Customers AUTO_INCREMENT = 1;");
        context.Database.ExecuteSqlRaw("ALTER TABLE Subscriptions AUTO_INCREMENT = 1;");
        context.Database.ExecuteSqlRaw("ALTER TABLE Products AUTO_INCREMENT = 1;");
        context.Database.ExecuteSqlRaw("ALTER TABLE Categories AUTO_INCREMENT = 1;");
        
        context.Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");

        // 1. ПІДПИСКИ
        var basicSub = new Subscription { Type = SubscriptionType.BasicCoffeePass, Price = 19.99m, DurationInDays = 30 };
        var premiumSub = new Subscription { Type = SubscriptionType.PremiumRoasterClub, Price = 49.99m, DurationInDays = 30 };
        context.Subscriptions.AddRange(basicSub, premiumSub);
        context.SaveChanges();

        // 2. КАТЕГОРІЇ
        var catCoffee = new Category { Name = "Coffee", Description = "Зернова та мелена кава з усього світу" };
        var catTea = new Category { Name = "Tea", Description = "Елітні сорти чаю" };
        var catAcc = new Category { Name = "Accessories", Description = "Обладнання для заварювання" };
        context.Categories.AddRange(catCoffee, catTea, catAcc);
        context.SaveChanges();

        // 3. ПРОДУКТИ (20 позицій з фронтенду)
        var products = new List<Product>
        {
            new Product { Name = "Ethiopia Sidamo", Price = 320m, Description = "яскраві нотки бергамоту та чорного чаю.", CategoryId = catCoffee.Id, StockQuantity = 50, RoastLevel = 3, Weight = 250 },
            new Product { Name = "Colombia Supremo", Price = 280m, Description = "класичний смак з відтінками карамелі та горіхів.", CategoryId = catCoffee.Id, StockQuantity = 45, RoastLevel = 4, Weight = 250 },
            new Product { Name = "Brazil Santos", Price = 240m, Description = "насичена кава з низькою кислотністю та шоколадним післясмаком.", CategoryId = catCoffee.Id, StockQuantity = 60, RoastLevel = 3, Weight = 250 },
            new Product { Name = "Kenya AA", Price = 450m, Description = "ексклюзивний сорт з винним ароматом та ягідним смаком.", CategoryId = catCoffee.Id, StockQuantity = 20, RoastLevel = 2, Weight = 250 },
            new Product { Name = "Guatemala Antigua", Price = 310m, Description = "димні нотки та глибокий смак какао.", CategoryId = catCoffee.Id, StockQuantity = 35, RoastLevel = 4, Weight = 250 },
            new Product { Name = "India Monsooned Malabar", Price = 290m, Description = "унікальна кава з мускусними та пряними відтінками.", CategoryId = catCoffee.Id, StockQuantity = 30, RoastLevel = 5, Weight = 250 },
            new Product { Name = "Costa Rica Tarrazu", Price = 340m, Description = "чиста чашка з фруктовим ароматом.", CategoryId = catCoffee.Id, StockQuantity = 25, RoastLevel = 3, Weight = 250 },
            new Product { Name = "Vietnam Robusta", Price = 180m, Description = "міцна кава з високим вмістом кофеїну.", CategoryId = catCoffee.Id, StockQuantity = 100, RoastLevel = 5, Weight = 500 },
            new Product { Name = "Oolong Tea", Price = 210m, Description = "традиційний китайський чай з ніжним молочним ароматом.", CategoryId = catTea.Id, StockQuantity = 40, RoastLevel = 0, Weight = 100 },
            new Product { Name = "Earl Grey", Price = 150m, Description = "чорний чай з додаванням олії бергамоту.", CategoryId = catTea.Id, StockQuantity = 80, RoastLevel = 0, Weight = 100 },
            new Product { Name = "Green Jasmine", Price = 190m, Description = "зелений чай з ніжними пелюстками жасмину.", CategoryId = catTea.Id, StockQuantity = 55, RoastLevel = 0, Weight = 100 },
            new Product { Name = "Pu-erh Royal", Price = 550m, Description = "витриманий чай з глибоким земляним смаком.", CategoryId = catTea.Id, StockQuantity = 15, RoastLevel = 0, Weight = 50 },
            new Product { Name = "Rwanda Bourbon", Price = 380m, Description = "солодкий профіль з нотками меду.", CategoryId = catCoffee.Id, StockQuantity = 22, RoastLevel = 2, Weight = 250 },
            new Product { Name = "Sumatra Mandheling", Price = 330m, Description = "низька кислотність та щільне тіло.", CategoryId = catCoffee.Id, StockQuantity = 18, RoastLevel = 5, Weight = 250 },
            new Product { Name = "Paper Filters", Price = 120m, Description = "набір з 100 паперових фільтрів для воронки.", CategoryId = catAcc.Id, StockQuantity = 200, RoastLevel = 0, Weight = 0.1 },
            new Product { Name = "Ceramic V60", Price = 850m, Description = "класична керамічна воронка для ручного заварювання.", CategoryId = catAcc.Id, StockQuantity = 12, RoastLevel = 0, Weight = 0.5 },
            new Product { Name = "Burr Grinder", Price = 2400m, Description = "ручний млинок з керамічними жорнами.", CategoryId = catAcc.Id, StockQuantity = 8, RoastLevel = 0, Weight = 0.8 },
            new Product { Name = "French Press", Price = 400m, Description = "простий спосіб заварювання насиченої кави.", CategoryId = catAcc.Id, StockQuantity = 15, RoastLevel = 0, Weight = 0.6 },
            new Product { Name = "Decaf Coffee", Price = 300m, Description = "100% арабіка без кофеїну.", CategoryId = catCoffee.Id, StockQuantity = 40, RoastLevel = 3, Weight = 250 },
            new Product { Name = "Specialty Blend", Price = 420m, Description = "авторська суміш від нашого обсмажувальника.", CategoryId = catCoffee.Id, StockQuantity = 25, RoastLevel = 4, Weight = 250 }
        };
        context.Products.AddRange(products);
        context.SaveChanges();

        // 4. КЛІЄНТИ
        var cust1 = new Customer { FirstName = "Ivan", LastName = "Franko", Email = "ivan@example.com", SubscriptionId = premiumSub.Id };
        var cust2 = new Customer { FirstName = "Taras", LastName = "Shevchenko", Email = "taras@example.com", SubscriptionId = basicSub.Id };
        var cust3 = new Customer { FirstName = "Lesya", LastName = "Ukrainka", Email = "lesya@example.com" };
        context.Customers.AddRange(cust1, cust2, cust3);
        context.SaveChanges();

        // 5. ТЕСТОВЕ ЗАМОВЛЕННЯ
        var order = new Order 
        { 
            OrderDate = DateTime.Now, 
            TotalAmount = products[0].Price + products[1].Price, 
            Status = OrderStatus.Paid,
            CustomerId = cust1.Id,
            Products = new List<Product> { products[0], products[1] }
        };
        context.Orders.Add(order);
        context.SaveChanges();
    }
}