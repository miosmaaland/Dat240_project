using System;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Products
{
    public class FoodItem : BaseEntity
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Picture Picture { get; set; } // URL or path to the picture

        // Constructor
        public FoodItem(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
            Picture = Picture.Default();
        }

        public void UpdatePicture(Picture picture)
        {
            Picture = picture ?? throw new ArgumentNullException(nameof(picture));
        }

    }

    public class FoodItemNameValidator : IValidator<FoodItem>
    {
        public (bool, string) IsValid(FoodItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item), "Cannot validate a null object");
            if (string.IsNullOrWhiteSpace(item.Name)) return (false, $"{nameof(item.Description)} cannot be empty");
            return (true, "");
        }
    }

    public class FoodItemDescriptionValidator : IValidator<FoodItem>
    {
        public (bool, string) IsValid(FoodItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item), "Cannot validate a null object");
            if (string.IsNullOrWhiteSpace(item.Description)) return (false, $"{nameof(item.Description)} cannot be empty");
            return (true, "");
        }
    }

    public class FoodItemPriceValidator : IValidator<FoodItem>
    {
        public (bool, string) IsValid(FoodItem item)
        {
            _ = item ?? throw new ArgumentNullException(nameof(item), "Cannot validate a null object");
            if (item.Price <= 0) return (false, $"{nameof(item.Price)} must be greater than 0");
            return (true, "");
        }
    }
}