namespace SmaHauJenHoaVij.Core.Domain.Ordering;
public class OrderLine
{
    public OrderLine(Guid foodItemId, string itemName, int amount, decimal price)
    {
        FoodItemId = foodItemId;
        ItemName = itemName;
        Amount = amount;
        Price = price;
    }

    public int Id { get; private set; }
    public Guid FoodItemId { get; private set; }
    public string ItemName { get; private set; }
    public int Amount { get; private set; }
    public decimal Price { get; private set; }
}
