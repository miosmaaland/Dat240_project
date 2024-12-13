using MediatR;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
public record OfferCourierSet : BaseDomainEvent
{
    public Guid OrderId { get; }
    public Guid CourierId { get; }

    public OfferCourierSet(Guid orderId, Guid courierId)
    {
        OrderId = orderId;
        CourierId = courierId;
    }
}
