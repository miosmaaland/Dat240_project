using System;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Invoicing;

public class InvoiceTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithValidData()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();
        var amount = 100m;
        var tip = 10m;

        // Act
        var invoice = new Invoice(customerId, orderId, amount, tip);

        // Assert
        Assert.NotEqual(Guid.Empty, invoice.Id); // Ensure a new Invoice ID is generated
        Assert.Equal(customerId, invoice.CustomerId); // Customer ID is set correctly
        Assert.Equal(orderId, invoice.OrderId); // Order ID is set correctly
        Assert.Equal(amount, invoice.Amount); // Amount is set correctly
        Assert.Equal(tip, invoice.Tip); // Tip is set correctly
        Assert.Equal(InvoiceStatus.New, invoice.Status); // Status should be 'New'
        Assert.Equal(amount + tip, invoice.TotalAmount); // Total amount should be sum of amount and tip
    }

    [Fact]
    public void MarkAsSent_ShouldUpdateStatusToSent()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);

        // Act
        invoice.MarkAsSent();

        // Assert
        Assert.Equal(InvoiceStatus.Sent, invoice.Status); // Status should be 'Sent'
    }

    [Fact]
    public void MarkAsSent_ShouldThrowException_WhenStatusIsNotNew()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);
        invoice.MarkAsSent();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => invoice.MarkAsSent()); // Should throw exception if status is not 'New'
    }

    [Fact]
    public void MarkAsPaid_ShouldUpdateStatusToPaid()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);
        invoice.MarkAsSent();

        // Act
        invoice.MarkAsPaid();

        // Assert
        Assert.Equal(InvoiceStatus.Paid, invoice.Status); // Status should be 'Paid'
    }

    [Fact]
    public void MarkAsPaid_ShouldThrowException_WhenStatusIsNotSent()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => invoice.MarkAsPaid()); // Should throw exception if status is not 'Sent'
    }

    [Fact]
    public void MarkAsCanceled_ShouldUpdateStatusToCancelled()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);

        // Act
        invoice.MarkAsCanceled();

        // Assert
        Assert.Equal(InvoiceStatus.Cancelled, invoice.Status); // Status should be 'Cancelled'
    }

    [Fact]
    public void MarkAsCanceled_ShouldThrowException_WhenStatusIsPaid()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);
        invoice.MarkAsSent();
        invoice.MarkAsPaid();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => invoice.MarkAsCanceled()); // Should throw exception if status is 'Paid'
    }

    [Fact]
    public void UpdateTip_ShouldUpdateTip_WhenTipIsValid()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);
        var newTip = 20m;

        // Act
        invoice.UpdateTip(newTip);

        // Assert
        Assert.Equal(newTip, invoice.Tip); // Tip should be updated
        Assert.Equal(120m, invoice.TotalAmount); // Total amount should be updated
    }

    [Fact]
    public void UpdateTip_ShouldThrowException_WhenTipIsNegative()
    {
        // Arrange
        var invoice = new Invoice(Guid.NewGuid(), Guid.NewGuid(), 100m);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => invoice.UpdateTip(-10m)); // Should throw exception if tip is negative
    }
}