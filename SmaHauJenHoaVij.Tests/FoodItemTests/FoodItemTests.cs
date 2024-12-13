using System;
using Xunit;
using SmaHauJenHoaVij.Core.Domain.Products;

public class FoodItemTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithDefaultPicture()
    {
        // Arrange
        var name = "Pizza";
        var description = "Delicious cheesy pizza";
        var price = 9.99m;

        // Act
        var foodItem = new FoodItem(name, description, price);

        // Assert
        Assert.Equal(name, foodItem.Name);
        Assert.Equal(description, foodItem.Description);
        Assert.Equal(price, foodItem.Price);
        Assert.NotNull(foodItem.Picture);
        Assert.Equal(Picture.Default().Path, foodItem.Picture.Path);
    }

    [Fact]
    public void UpdatePicture_ShouldUpdatePicture()
    {
        // Arrange
        var foodItem = new FoodItem("Pizza", "Delicious cheesy pizza", 9.99m);
        var newPicture = Picture.FromPath("/images/fooditems/pizza.jpg");

        // Act
        foodItem.UpdatePicture(newPicture);

        // Assert
        Assert.Equal(newPicture.Path, foodItem.Picture.Path);
    }

    [Fact]
    public void UpdatePicture_ShouldThrowArgumentNullException_WhenPictureIsNull()
    {
        // Arrange
        var foodItem = new FoodItem("Pizza", "Delicious cheesy pizza", 9.99m);

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => foodItem.UpdatePicture(null));
    }

    [Fact]
    public void FoodItemNameValidator_ShouldReturnFalse_WhenNameIsNullOrEmpty()
    {
        // Arrange
        var validator = new FoodItemNameValidator();
        var foodItem = new FoodItem("", "Description", 9.99m);

        // Act
        var (isValid, error) = validator.IsValid(foodItem);

        // Assert
        Assert.False(isValid);
        Assert.Equal("Description cannot be empty", error);
    }

    [Fact]
    public void FoodItemDescriptionValidator_ShouldReturnFalse_WhenDescriptionIsNullOrEmpty()
    {
        // Arrange
        var validator = new FoodItemDescriptionValidator();
        var foodItem = new FoodItem("Name", "", 9.99m);

        // Act
        var (isValid, error) = validator.IsValid(foodItem);

        // Assert
        Assert.False(isValid);
        Assert.Equal("Description cannot be empty", error);
    }

    [Fact]
    public void FoodItemPriceValidator_ShouldReturnFalse_WhenPriceIsZeroOrNegative()
    {
        // Arrange
        var validator = new FoodItemPriceValidator();
        var foodItem = new FoodItem("Name", "Description", -1m);

        // Act
        var (isValid, error) = validator.IsValid(foodItem);

        // Assert
        Assert.False(isValid);
        Assert.Equal("Price must be greater than 0", error);
    }

    [Fact]
    public void FoodItemPriceValidator_ShouldReturnTrue_WhenPriceIsPositive()
    {
        // Arrange
        var validator = new FoodItemPriceValidator();
        var foodItem = new FoodItem("Name", "Description", 10m);

        // Act
        var (isValid, error) = validator.IsValid(foodItem);

        // Assert
        Assert.True(isValid);
        Assert.Equal(string.Empty, error);
    }
}
