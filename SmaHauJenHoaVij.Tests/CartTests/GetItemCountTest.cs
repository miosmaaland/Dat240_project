using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Cart;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using MediatR;

public class GetItemCountTests
{
    [Fact]
    public async Task Handle_ShouldReturnItemCount()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, Guid.NewGuid());
        cart.AddItem(Guid.NewGuid(), "TestItem1", 50m);
        cart.AddItem(Guid.NewGuid(), "TestItem2", 100m);

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetItemCount_ShouldReturnItemCount")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new GetItemCount.Request(cartId);
        var handler = new GetItemCount.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result); // Two items in the cart
    }

    [Fact]
    public async Task Handle_ShouldReturnZero_WhenCartIsEmpty()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, Guid.NewGuid());

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetItemCount_ShouldReturnZero")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new GetItemCount.Request(cartId);
        var handler = new GetItemCount.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result); // No items in the cart
    }

    [Fact]
    public async Task Handle_ShouldReturnZero_WhenCartDoesNotExist()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetItemCount_ShouldReturnZeroWhenCartDoesNotExist")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        var request = new GetItemCount.Request(cartId);
        var handler = new GetItemCount.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(0, result); // No cart, no items
    }
}
