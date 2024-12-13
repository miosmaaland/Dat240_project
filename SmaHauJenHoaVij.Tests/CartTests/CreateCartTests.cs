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

public class CreateCartTests
{
    [Fact]
    public async Task Handle_ShouldCreateNewCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "CreateCart_ShouldCreateNewCart")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        var request = new CreateCart.Request(cartId, customerId);
        var handler = new CreateCart.Handler(context);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var createdCart = await context.ShoppingCarts.SingleAsync(c => c.Id == cartId);
        Assert.NotNull(createdCart);
        Assert.Equal(customerId, createdCart.CustomerId);
    }
}
