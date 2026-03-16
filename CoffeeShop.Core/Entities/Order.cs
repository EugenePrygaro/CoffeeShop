namespace CoffeeShop.Core.Entities;

public enum OrderStatus
{
    New,        // 0 - Нове
    Paid,       // 1 - Оплачене
    Delivered   // 2 - Доставлене
}

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
    public int CustomerId { get; set; }
    
    // Масив продуктів, які належать до цього замовлення
    public List<Product> Products { get; set; } = new();

    // Метод для безпечної зміни статусу із зашитими правилами бізнес-логіки
    public void ChangeStatus(OrderStatus newStatus)
    {
        if (Status == OrderStatus.Delivered)
        {
            throw new InvalidOperationException("Неможливо змінити статус: замовлення вже доставлено.");
        }

        if (Status == OrderStatus.New && newStatus == OrderStatus.Delivered)
        {
            throw new InvalidOperationException("Порушення бізнес-логіки: замовлення має бути оплачено (Paid) перед доставкою.");
        }

        if (Status == OrderStatus.Paid && newStatus == OrderStatus.New)
        {
            throw new InvalidOperationException("Оплачене замовлення не може знову стати новим.");
        }

        Status = newStatus;
    }
}