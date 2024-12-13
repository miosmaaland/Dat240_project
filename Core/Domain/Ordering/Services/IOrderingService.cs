using System;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;


namespace SmaHauJenHoaVij.Core.Domain.Ordering.Services;

public interface IOrderingService
{
    Task<Guid> PlaceOrder(Guid customerId, Location location, OrderLineDto[] orderLines);
    Task CancelOrder(Guid orderId);
}
