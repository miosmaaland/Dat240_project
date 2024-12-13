namespace SmaHauJenHoaVij.Models;

public class AdminOrderViewModel
{
    public Guid OrderId { get; set; }
    public string CustomerName { get; set; }
    public string Status { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
}