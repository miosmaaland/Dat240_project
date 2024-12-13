using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;
using SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Handlers
{
    public class PlaceOrderHandler : IRequestHandler<PlaceOrder.Request, PlaceOrder.Response>
    {
        private readonly IOrderingService _orderingService;

        public PlaceOrderHandler(IOrderingService orderingService)
        {
            _orderingService = orderingService;
        }

        public async Task<PlaceOrder.Response> Handle(PlaceOrder.Request request, CancellationToken cancellationToken)
        {
            var location = new Location(request.Location.Building, request.Location.RoomNumber, request.Location.Notes);

            try
            {
                var orderId = await _orderingService.PlaceOrder(
                    request.CustomerId, // Use the correct CustomerId
                    location,
                    request.OrderLines.ToArray()
                );

                return new PlaceOrder.Response(orderId, true, "Order placed successfully.");
            }
            catch (Exception ex)
            {
                return new PlaceOrder.Response(Guid.Empty, false, $"Error placing order: {ex.Message}");
            }
        }
    }
}
