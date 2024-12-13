namespace SmaHauJenHoaVij.Models;

public class CartItemViewModel
{
    public Guid Sku { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Count { get; set; }
    public decimal TotalPrice => Price * Count;
    public decimal DeiliveryFee { get; set; }
    public string? PicturePath { get; set; }
}