using System;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Fulfillment;

public class OfferTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithValidData()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reimbursementId = Guid.NewGuid();

        // Act
        var offer = new Offer(orderId, reimbursementId);

        // Assert
        Assert.NotEqual(Guid.Empty, offer.Id); // Ensure a new Offer ID is generated
        Assert.Equal(orderId, offer.OrderId); // Order ID is set correctly
        Assert.Null(offer.CourierId); // Courier should be unassigned initially
        Assert.Equal(reimbursementId, offer.Reimbursement); // Reimbursement ID should be correct
        Assert.Equal(0, offer.Tip); // Initial tip should be 0
    }

    [Fact]
    public void AssignCourier_ShouldUpdateCourierId()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reimbursementId = Guid.NewGuid();
        var offer = new Offer(orderId, reimbursementId);
        var courierId = Guid.NewGuid();

        // Act
        offer.AssignCourier(courierId);

        // Assert
        Assert.Equal(courierId, offer.CourierId); // Check if the courier ID was updated correctly
    }

    [Fact]
    public void AssignCourier_ShouldThrowArgumentException_WhenCourierIdIsEmpty()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reimbursementId = Guid.NewGuid();
        var offer = new Offer(orderId, reimbursementId);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => offer.AssignCourier(Guid.Empty)); // Empty GUID should throw exception
    }

    [Fact]
    public void AssignTip_ShouldUpdateTip_WhenTipIsValid()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reimbursementId = Guid.NewGuid();
        var offer = new Offer(orderId, reimbursementId);
        var tipAmount = 15.75m;

        // Act
        offer.AssignTip(tipAmount);

        // Assert
        Assert.Equal(tipAmount, offer.Tip); // Ensure the tip is correctly updated
    }

    [Fact]
    public void AssignTip_ShouldThrowArgumentException_WhenTipIsNegative()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reimbursementId = Guid.NewGuid();
        var offer = new Offer(orderId, reimbursementId);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => offer.AssignTip(-5m)); // Negative tip should throw exception
    }

    [Fact]
    public void AssignTip_ShouldThrowArgumentException_WhenTipIsTooLarge()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var reimbursementId = Guid.NewGuid();
        var offer = new Offer(orderId, reimbursementId);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => offer.AssignTip(10000m)); // Very large tip should throw exception
    }
}
