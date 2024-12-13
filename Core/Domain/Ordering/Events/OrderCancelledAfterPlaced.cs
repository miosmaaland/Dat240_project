using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Events
{
    public record OrderCancelledAfterPlaced(Guid OrderId) : BaseDomainEvent;
}
