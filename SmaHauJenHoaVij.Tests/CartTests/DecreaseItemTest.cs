using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Cart;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using MediatR;

public class DecreaseItemTests
{
    [Fact]
    public async Task Handle_ShouldDecreaseItemCount()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var sku = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, Guid.NewGuid());
        cart.AddItem(sku, "TestItem", 100m);  // Assuming each AddItem defaults to a count of 1

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "DecreaseItem_ShouldDecreaseItemCount")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new DecreaseItem.Request(cartId, sku);
        var handler = new DecreaseItem.Handler(context);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedCart = await context.ShoppingCarts.Include(c => c.Items)
                                .SingleAsync(c => c.Id == cartId);

        // Check if the item still exists
        var item = updatedCart.Items.SingleOrDefault(i => i.Sku == sku);
        
        if (item != null)
        {
            // Item still exists, so assert that the count decreased to zero
            Assert.Equal(0, item.Count);
        }
        else
        {
            // If the item is removed, it means the count reached zero
            Assert.Empty(updatedCart.Items);
        }
    }


    [Fact]
    public async Task Handle_ShouldRemoveItemWhenCountReachesZero()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var sku = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, Guid.NewGuid());
        cart.AddItem(sku, "TestItem", 100m);

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "DecreaseItem_ShouldRemoveItem")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new DecreaseItem.Request(cartId, sku);
        var handler = new DecreaseItem.Handler(context);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedCart = await context.ShoppingCarts.Include(c => c.Items)
                                .SingleAsync(c => c.Id == cartId);
        Assert.Empty(updatedCart.Items);
    }
}
