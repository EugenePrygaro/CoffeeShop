using Microsoft.EntityFrameworkCore;
using CoffeeShop.Core.Common;
using CoffeeShop.Core.Catalog;
using CoffeeShop.Core.Customers;
using CoffeeShop.Core.Sales;
using CoffeeShop.Core.Loyalty;

namespace CoffeeShop.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<PromoAction> PromoActions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Налаштування для збереження Value Object (Money) в таблиці Orders
        modelBuilder.Entity<Order>(builder =>
        {
            builder.OwnsOne(o => o.TotalAmount, money =>
            {
                money.Property(m => m.Amount).HasColumnName("TotalAmount");
                money.Property(m => m.Currency).HasColumnName("Currency").HasMaxLength(3);
            });
        });
    }

    // Перехоплюємо збереження для диспетчеризації доменних подій
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = ChangeTracker
            .Entries<AggregateRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        // Очищаємо події, щоб уникнути повторної відправки
        domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Відправляємо події (імітація для перевірки)
        foreach (var domainEvent in domainEvents)
        {
            Console.WriteLine($"[DOMAIN EVENT DISPATCHED]: {domainEvent.GetType().Name}");
        }

        return result;
    }
}