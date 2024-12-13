using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment;
public class Offer : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid? CourierId { get; private set; } // Nullable to represent unassigned state
    public Guid Reimbursement { get; private set; }
    public decimal Tip { get; private set; }
   

    private Offer() { } // EF Core

    public Offer(Guid orderId, Guid reimbursementId)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        CourierId = null;
        Reimbursement = reimbursementId;
        Tip = 0;
    }

    public void AssignCourier(Guid courierId)
    {
        if (courierId == Guid.Empty)
        {
            throw new ArgumentException("Courier ID cannot be empty.");
        }
        CourierId = courierId;
    }

    public void AssignTip(decimal tipAmount)
    {
        if (tipAmount < 0)
        {
            throw new ArgumentException("Tip cannot be negative.");
        }
        if (tipAmount > 1000) // Assuming the tip limit is 1000
        {
            throw new ArgumentException("Tip is too large.");
        }
        Tip = tipAmount;
    }
}