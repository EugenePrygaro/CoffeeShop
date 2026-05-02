using CoffeeShop.Core.Common;
using CoffeeShop.Core.ValueObjects;
using CoffeeShop.Core.Catalog; // Для доступу до Product

namespace CoffeeShop.Core.Sales;

// Доменна подія, що генерується при створенні замовлення
public record OrderCreatedEvent(int OrderId) : IDomainEvent;

public enum OrderStatus
{
    New = 0,
    Paid = 1,
    Delivered = 2
}

public class Order : AggregateRoot // Успадковуємо базовий клас Агрегату
{
    // Всі сеттери стали private для інкапсуляції
    public int Id { get; private set; }
    public DateTime OrderDate { get; private set; }
    
    // Замінили decimal на Value Object Money
    public Money TotalAmount { get; private set; }
    
    public OrderStatus Status { get; private set; }
    public int CustomerId { get; private set; }
    
    // Інкапсулюємо колекцію продуктів
    private readonly List<Product> _products = new();
    public IReadOnlyList<Product> Products => _products.AsReadOnly();

    // Порожній конструктор потрібен для Entity Framework
    private Order() { }

    // Фабричний метод для створення замовлення (замість використання new Order())
    public static Order Create(int customerId)
    {
        var order = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.New,
            TotalAmount = Money.Create(0) // Початкова сума 0 UAH
        };

        // Збуджуємо доменну подію згідно з вимогами ЛР №4
        order.RaiseDomainEvent(new OrderCreatedEvent(order.Id));
        
        return order;
    }

    // Метод для додавання продукту до замовлення
    public void AddProduct(Product product)
    {
        if (Status != OrderStatus.New)
        {
            throw new InvalidOperationException("Товари можна додавати лише до нового замовлення.");
        }

        _products.Add(product);
        
        // Перераховуємо загальну суму безпечно
        TotalAmount = Money.Create(TotalAmount.Amount + product.Price);
    }

    // Твій існуючий метод залишається, тільки сеттер Status ми закрили
    public void ChangeStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Delivered)
            throw new InvalidOperationException("Неможливо змінити статус: замовлення вже доставлено.");

        if (Status == OrderStatus.New && newStatus == OrderStatus.Delivered)
            throw new InvalidOperationException("Порушення: замовлення має бути оплачено (Paid) перед доставкою.");

        if (Status == OrderStatus.Paid && newStatus == OrderStatus.New)
            throw new InvalidOperationException("Оплачене замовлення не може знову стати новим.");

        Status = newStatus;
    }
}