using Xunit;
using CoffeeShop.Core.Sales; // Змінили на новий простір імен
using System;

namespace CoffeeShop.Tests;

public class OrderStatusTests
{
    // Тест 1: Успішний перехід (позитивний сценарій)
    [Fact]
    public void ChangeStatus_FromNewToPaid_ShouldUpdateStatus()
    {
        // Arrange: створюємо нове замовлення через фабричний метод (статус автоматично New)
        var order = Order.Create(customerId: 1);

        // Act: намагаємося змінити статус на оплачений
        order.ChangeStatus(OrderStatus.Paid);

        // Assert: статус має стати Paid
        Assert.Equal(OrderStatus.Paid, order.Status);
    }

    // Тест 2: Спроба перескочити статус (негативний сценарій)
    [Fact]
    public void ChangeStatus_FromNewToDelivered_ShouldThrowException()
    {
        // Arrange
        var order = Order.Create(customerId: 1);

        // Act & Assert
        // Оскільки метод має викинути помилку, ми "ловимо" її за допомогою Assert.Throws
        var exception = Assert.Throws<InvalidOperationException>(() => order.ChangeStatus(OrderStatus.Delivered));

        // Перевіряємо, чи правильний текст помилки повернувся
        Assert.Contains("має бути оплачено", exception.Message);
    }

    // Тест 3: Крок назад заборонено (негативний сценарій)
    [Fact]
    public void ChangeStatus_FromPaidToNew_ShouldThrowException()
    {
        // Arrange: створюємо замовлення і легально переводимо його в статус Paid
        var order = Order.Create(customerId: 1);
        order.ChangeStatus(OrderStatus.Paid);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => order.ChangeStatus(OrderStatus.New));
        Assert.Contains("не може знову стати новим", exception.Message);
    }

    // Тест 4: Зміна доставленого замовлення заборонена
    [Fact]
    public void ChangeStatus_WhenAlreadyDelivered_ShouldThrowException()
    {
        // Arrange: створюємо замовлення і легально проводимо його до статусу Delivered
        var order = Order.Create(customerId: 1);
        order.ChangeStatus(OrderStatus.Paid);      // Спочатку оплачуємо
        order.ChangeStatus(OrderStatus.Delivered); // Потім доставляємо

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => order.ChangeStatus(OrderStatus.Paid));
        Assert.Contains("вже доставлено", exception.Message);
    }
}