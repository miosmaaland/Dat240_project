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

public class GetCartByCustomerIdTests
{
    [Fact]
    public async Task Handle_ShouldReturnCart_WhenCustomerIdExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var cart = new ShoppingCart(Guid.NewGuid(), customerId);

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetCartByCustomerId_ShouldReturnCart")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new GetCartByCustomerId.Request(customerId);
        var handler = new GetCartByCustomerId.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(customerId, result?.CustomerId);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenCustomerIdDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "GetCartByCustomerId_ShouldReturnNull")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        var request = new GetCartByCustomerId.Request(customerId);
        var handler = new GetCartByCustomerId.Handler(context);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
