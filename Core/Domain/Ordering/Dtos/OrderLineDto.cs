namespace SmaHauJenHoaVij.Core.Domain.Ordering.Dtos
{
    public record OrderLineDto(
        Guid FoodItemId, // Changed to Guid
        string FoodItemName,
        int Amount,
        decimal Price
    );
}
