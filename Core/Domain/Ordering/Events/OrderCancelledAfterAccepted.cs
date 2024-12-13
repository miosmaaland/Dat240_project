using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Events
{
    public record OrderCancelledAfterAccepted(Guid OrderId) : BaseDomainEvent;
}
