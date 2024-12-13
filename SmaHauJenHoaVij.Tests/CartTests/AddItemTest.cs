using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Cart;
using Moq;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Cart.Pipelines;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AddItemTests
{
    [Fact]
    public async Task Handle_ShouldAddItemToCart()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var sku = Guid.NewGuid();
        var cart = new ShoppingCart(cartId, customerId);

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "AddItem_ShouldAddItemToCart")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        context.ShoppingCarts.Add(cart);
        await context.SaveChangesAsync();

        var request = new AddItem.Request(cartId, customerId, sku, "TestItem", 100m);
        var handler = new AddItem.Handler(context);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var updatedCart = await context.ShoppingCarts.Include(c => c.Items)
                                .SingleAsync(c => c.Id == cartId);
        Assert.NotEmpty(updatedCart.Items);
        Assert.Equal(sku, updatedCart.Items.First().Sku);
        Assert.Equal(1, updatedCart.Items.First().Count);
    }

    [Fact]
    public async Task Handle_ShouldCreateNewCartIfNotExists()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var sku = Guid.NewGuid();

        var options = new DbContextOptionsBuilder<ShopContext>()
            .UseInMemoryDatabase(databaseName: "AddItem_ShouldCreateNewCartIfNotExists")
            .Options;

        var mediator = new Mock<IMediator>().Object;
        using var context = new ShopContext(options, mediator);
        var request = new AddItem.Request(cartId, customerId, sku, "NewItem", 50m);
        var handler = new AddItem.Handler(context);

        // Act
        await handler.Handle(request, CancellationToken.None);

        // Assert
        var newCart = await context.ShoppingCarts.Include(c => c.Items)
                                .SingleAsync(c => c.Id == cartId);
        Assert.NotNull(newCart);
        Assert.Single(newCart.Items);
        Assert.Equal(sku, newCart.Items.First().Sku);
    }
}
