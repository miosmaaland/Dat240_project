using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Ordering.Events;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using SmaHauJenHoaVij.SharedKernel;
using SmaHauJenHoaVij.Core.Domain.Ordering.Services;
using SmaHauJenHoaVij.Core.Domain.Ordering;
using SmaHauJenHoaVij.Pages.AdminPages.Orders;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Services
{
    public class OrderingService : IOrderingService
    {
        private readonly ShopContext _db;
        private readonly DeliveryFeeService _deliveryFeeService;

        public OrderingService(ShopContext db, DeliveryFeeService deliveryFeeService)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _deliveryFeeService = deliveryFeeService ?? throw new ArgumentNullException(nameof(deliveryFeeService));
        }

        public async Task<Guid> PlaceOrder(Guid customerId, Location location, OrderLineDto[] orderLines)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer == null)
            {
                throw new ArgumentException("Customer not found");
            }

            // Fetch the current delivery fee
            var deliveryFee = _deliveryFeeService.GetCurrentFee();

            var orderId = Guid.NewGuid(); // Generate a unique ID for the order
            var order = new Order(orderId, location, customer.Id, customer.Name, deliveryFee);

            foreach (var orderLineDto in orderLines)
            {
                order.AddOrderLine(orderLineDto.FoodItemId, orderLineDto.FoodItemName, orderLineDto.Amount, orderLineDto.Price);
            }

            var totalAmount = order.GetTotalPrice();

            var orderPlacedEvent = new OrderPlaced(orderId, customerId, customer.Name, totalAmount);
            order.Events.Add(orderPlacedEvent);

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return order.Id;
        }

        public async Task CancelOrder(Guid orderId)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            if (order.Status == Status.Placed)
            {
                order.MarkAsCancelled();
                order.Events.Add(new OrderCancelledAfterPlaced(orderId));
            }
            else if (order.Status == Status.Accepted)
            {
                order.MarkAsCancelled();
                order.Events.Add(new OrderCancelledAfterAccepted(orderId));
            }
            else
            {
                throw new InvalidOperationException("Order cannot be cancelled at this stage.");
            }

            await _db.SaveChangesAsync();
        }

    }
}
