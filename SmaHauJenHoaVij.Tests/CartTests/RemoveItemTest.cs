using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Cart;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class RemoveItemTests
{
    [Fact]
    public async Task Handle_ShouldRemoveItemFromCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var sku = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, Guid.NewGuid());
        cart.AddItem(sku, "TestItem", 50m);

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "RemoveItem_ShouldRemoveItemFromCart")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new RemoveItem.Request(cartId, sku);
        var handler = new RemoveItem.Handler(context);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedCart = await context.ShoppingCarts.Include(c => c.Items)
                                .SingleAsync(c => c.Id == cartId);
        Assert.Empty(updatedCart.Items);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenCartDoesNotExist()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var sku = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "RemoveItem_ShouldThrowException")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        var request = new RemoveItem.Request(cartId, sku);
        var handler = new RemoveItem.Handler(context);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
    }
}
