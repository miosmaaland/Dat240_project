using System;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Cart;
using System.Linq;

public class ShoppingCartTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithValidData()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        // Act
        var shoppingCart = new ShoppingCart(cartId, customerId);

        // Assert
        Assert.Equal(cartId, shoppingCart.Id); // Cart ID should be set correctly
        Assert.Equal(customerId, shoppingCart.CustomerId); // Customer ID should be set correctly
        Assert.Empty(shoppingCart.Items); // ShoppingCart should be empty at initialization
    }

    [Fact]
    public void AddItem_ShouldAddNewItem_WhenItemDoesNotExist()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var shoppingCart = new ShoppingCart(cartId, customerId);
        var sku = Guid.NewGuid();
        var name = "Laptop";
        var price = 1000m;

        // Act
        shoppingCart.AddItem(sku, name, price);

        // Assert
        var item = shoppingCart.Items.SingleOrDefault(i => i.Sku == sku);
        Assert.NotNull(item); // Ensure the item was added
        Assert.Equal(name, item.Name); // Check the name
        Assert.Equal(price, item.Price); // Check the price
        Assert.Equal(1, item.Count); // Ensure the quantity is 1
    }

    [Fact]
    public void AddItem_ShouldIncreaseQuantity_WhenItemAlreadyExists()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var shoppingCart = new ShoppingCart(cartId, customerId);
        var sku = Guid.NewGuid();
        var name = "Laptop";
        var price = 1000m;

        // Act
        shoppingCart.AddItem(sku, name, price);
        shoppingCart.AddItem(sku, name, price); // Add the same item again

        // Assert
        var item = shoppingCart.Items.SingleOrDefault(i => i.Sku == sku);
        Assert.NotNull(item); // Ensure the item exists
        Assert.Equal(2, item.Count); // Quantity should be 2 now
    }

    [Fact]
    public void RemoveItem_ShouldRemoveItem_WhenItemExists()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var shoppingCart = new ShoppingCart(cartId, customerId);
        var sku = Guid.NewGuid();
        var name = "Laptop";
        var price = 1000m;

        shoppingCart.AddItem(sku, name, price); // Add an item first

        // Act
        shoppingCart.RemoveItem(sku); // Remove the item

        // Assert
        var item = shoppingCart.Items.SingleOrDefault(i => i.Sku == sku);
        Assert.Null(item); // Item should be removed from the cart
    }

    [Fact]
    public void RemoveItem_ShouldDoNothing_WhenItemDoesNotExist()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var shoppingCart = new ShoppingCart(cartId, customerId);
        var sku = Guid.NewGuid(); // Non-existing SKU

        // Act
        shoppingCart.RemoveItem(sku); // Try to remove a non-existing item

        // Assert
        Assert.Empty(shoppingCart.Items); // Cart should still be empty
    }

    [Fact]
    public void ClearCart_ShouldEmptyCart_WhenCalled()
    {
        // Arrange
        var cartId = Guid.NewGuid();
        var customerId = Guid.NewGuid();
        var shoppingCart = new ShoppingCart(cartId, customerId);
        var sku1 = Guid.NewGuid();
        var sku2 = Guid.NewGuid();

        shoppingCart.AddItem(sku1, "Laptop", 1000m);
        shoppingCart.AddItem(sku2, "Phone", 500m);

        // Act
        shoppingCart.ClearCart(); // Clear the cart

        // Assert
        Assert.Empty(shoppingCart.Items); // Cart should be empty
    }
}
