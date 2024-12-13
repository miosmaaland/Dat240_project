using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;
using SmaHauJenHoaVij.Core.Domain.Cart;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;
using Microsoft.EntityFrameworkCore;

public class CartCheckoutTests
{
    [Fact]
    public async Task Handle_ShouldCheckoutCartAndReturnOrderId()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cartId = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, customerId);
        cart.AddItem(Guid.NewGuid(), "TestItem", 100m);

        var location = new Location("123 Street", "City", "Country");
        var orderId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "CartCheckout_ShouldReturnOrderId")
            .Options;

        var mockMediator = new Mock<IMediator>();
        using var context = new ShopContext(options, mockMediator.Object);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var mockOrderingService = new Mock<IOrderingService>();
        mockOrderingService
            .Setup(service => service.PlaceOrder(customerId, location, It.IsAny<SmaHauJenHoaVij.Core.Domain.Ordering.Dtos.OrderLineDto[]>()))
            .ReturnsAsync(orderId);

        var request = new CartCheckout.Request(customerId, location);
        var handler = new CartCheckout.Handler(context, mockOrderingService.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(orderId, result);
        Assert.False(context.ShoppingCarts.Any()); // Cart should be removed after checkout
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCartDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var location = new Location("123 Street", "City", "Country");

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "CartCheckout_ShouldThrowException")
            .Options;

        var mockMediator = new Mock<IMediator>();
        using var context = new ShopContext(options, mockMediator.Object);
        var mockOrderingService = new Mock<IOrderingService>();
        var request = new CartCheckout.Request(customerId, location);
        var handler = new CartCheckout.Handler(context, mockOrderingService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
    }
}
