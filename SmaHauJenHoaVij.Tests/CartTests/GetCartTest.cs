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

public class GetCartTests
{
    [Fact]
    public async Task Handle_ShouldReturnCart_WhenCartExists()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, Guid.NewGuid());

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetCart_ShouldReturnCart")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new GetCart.Request(cartId);
        var handler = new GetCart.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cartId, result?.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCartDoesNotExist()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetCart_ShouldReturnNull")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        var request = new GetCart.Request(cartId);
        var handler = new GetCart.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
