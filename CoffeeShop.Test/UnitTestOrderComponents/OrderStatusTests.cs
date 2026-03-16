using Xunit;
using CoffeeShop.Core.Entities;
using System;

namespace CoffeeShop.Tests;

public class OrderStatusTests
{
    // Тест 1: Успішний перехід (позитивний сценарій)
    [Fact]
    public void ChangeStatus_FromNewToPaid_ShouldUpdateStatus()
    {
        // Arrange: створюємо нове замовлення
        var order = new Order { Status = OrderStatus.New };

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
        var order = new Order { Status = OrderStatus.New };

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
        // Arrange: замовлення вже оплачене
        var order = new Order { Status = OrderStatus.Paid };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => order.ChangeStatus(OrderStatus.New));
        Assert.Contains("не може знову стати новим", exception.Message);
    }

    // Тест 4: Зміна доставленого замовлення заборонена
    [Fact]
    public void ChangeStatus_WhenAlreadyDelivered_ShouldThrowException()
    {
        // Arrange: замовлення вже доставлене клієнту
        var order = new Order { Status = OrderStatus.Delivered };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => order.ChangeStatus(OrderStatus.Paid));
        Assert.Contains("вже доставлено", exception.Message);
    }
}