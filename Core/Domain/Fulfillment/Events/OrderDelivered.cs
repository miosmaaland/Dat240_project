using MediatR;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
public record OrderDelivered : BaseDomainEvent
{
    public Guid OrderId { get; }
    public Guid CourierId { get; }

    public OrderDelivered(Guid orderId, Guid courierId)
    {
        OrderId = orderId;
        CourierId = courierId;
    }
}
