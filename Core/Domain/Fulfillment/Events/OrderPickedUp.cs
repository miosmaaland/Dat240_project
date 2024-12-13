using MediatR;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
public record OrderPickedUp : BaseDomainEvent
{
    public Guid OrderId { get; }
    public Guid CourierId { get; }

    public OrderPickedUp(Guid orderId, Guid courierId)
    {
        OrderId = orderId;
        CourierId = courierId;
    }
}
