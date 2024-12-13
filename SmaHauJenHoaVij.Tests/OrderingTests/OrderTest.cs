using System;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Ordering;

public class OrderTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithValidData()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);

        // Act
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);

        // Assert
        Assert.Equal(orderId, order.Id);
        Assert.Equal(location, order.Location);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(customerName, order.CustomerName);
        Assert.Equal(Status.Placed, order.Status);
        Assert.Equal(deliveryFee, order.DeliveryFee);
        Assert.Empty(order.OrderLines);
    }

    [Fact]
    public void AddOrderLine_ShouldAddNewOrderLine()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);

        var foodItemId = Guid.NewGuid();
        var foodItemName = "Pizza";
        var amount = 2;
        var price = 10.00m;

        // Act
        order.AddOrderLine(foodItemId, foodItemName, amount, price);

        // Assert
        var orderLine = Assert.Single(order.OrderLines);
        Assert.Equal(foodItemId, orderLine.FoodItemId);
        Assert.Equal(foodItemName, orderLine.ItemName);
        Assert.Equal(amount, orderLine.Amount);
        Assert.Equal(price, orderLine.Price);
    }

    [Fact]
    public void GetTotalPrice_ShouldReturnCorrectTotal()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);

        order.AddOrderLine(Guid.NewGuid(), "Pizza", 2, 10.00m);
        order.AddOrderLine(Guid.NewGuid(), "Burger", 1, 5.00m);

        // Act
        var totalPrice = order.GetTotalPrice();

        // Assert
        Assert.Equal(30.00m, totalPrice);
    }

    [Fact]
    public void MarkAsAccepted_ShouldUpdateStatusToAccepted()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);

        // Act
        order.MarkAsAccepted();

        // Assert
        Assert.Equal(Status.Accepted, order.Status);
    }

    [Fact]
    public void MarkAsPicked_Up_ShouldUpdateStatusToPicked_Up()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);
        order.MarkAsAccepted();

        // Act
        order.MarkAsPicked_Up();

        // Assert
        Assert.Equal(Status.Picked_Up, order.Status);
    }

    [Fact]
    public void MarkAsDelivered_ShouldUpdateStatusToDelivered()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);
        order.MarkAsAccepted();
        order.MarkAsPicked_Up();

        // Act
        order.MarkAsDelivered();

        // Assert
        Assert.Equal(Status.Delivered, order.Status);
    }

    [Fact]
    public void MarkAsCancelled_ShouldUpdateStatusToCancelled()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);

        // Act
        order.MarkAsCancelled();

        // Assert
        Assert.Equal(Status.Canceled, order.Status);
    }

    [Fact]
    public void MarkAsCancelled_ShouldThrowException_WhenOrderIsDelivered()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var location = new Location("123 Street", "12345", "hei");
        var customerId = Guid.NewGuid();
        var customerName = "John Doe";
        var deliveryFee = new DeliveryFee(5.00m);
        var order = new Order(orderId, location, customerId, customerName, deliveryFee);
        order.MarkAsAccepted();
        order.MarkAsPicked_Up();
        order.MarkAsDelivered();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => order.MarkAsCancelled());
    }
}