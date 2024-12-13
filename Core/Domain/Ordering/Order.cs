using System;
using System.Collections.Generic;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Ordering
{
    public class Order : BaseEntity
    {
        private Order() { }

        public Order(Guid orderId, Location location, Guid customerId, string customerName, DeliveryFee deliveryFee)
        {
            Id = orderId;
            Location = location ?? throw new ArgumentNullException(nameof(location));
            CustomerId = customerId;
            CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
            Status = Status.Placed;
            OrderDate = DateTime.UtcNow;
            Notes = string.Empty;
            DeliveryFee = deliveryFee ?? throw new ArgumentNullException(nameof(deliveryFee));
        }

        public Guid Id { get; private set; } // Unique identifier for the order
        public Guid CustomerId { get; private set; } // References the customer placing the order
        public string CustomerName { get; private set; }
        public DateTime OrderDate { get; private set; }
        public Location Location { get; private set; }
        public string Notes { get; private set; }
        public Status Status { get; private set; }
        public DeliveryFee DeliveryFee { get; private set; }

        private readonly List<OrderLine> _orderLines = new();
        public IReadOnlyCollection<OrderLine> OrderLines => _orderLines.AsReadOnly();

        public void AddOrderLine(Guid foodItemId, string foodItemName, int amount, decimal price)
        {
            _orderLines.Add(new OrderLine(foodItemId, foodItemName, amount, price));
        }


        public decimal GetTotalPrice()
        {
            return _orderLines.Sum(ol => ol.Price * ol.Amount) + DeliveryFee.Value;
        }

        public void MarkAsAccepted()
        {
            if (Status != Status.Placed)
                throw new InvalidOperationException("Order must be placed to be accepted.");

            Status = Status.Accepted;
        }

        public void MarkAsPicked_Up()
        {
            if (Status != Status.Accepted)
                throw new InvalidOperationException("Order must be accepted to be picked up.");

            Status = Status.Picked_Up;
        }

        public void MarkAsDelivered()
        {
            if (Status != Status.Picked_Up)
                throw new InvalidOperationException("Order must be picked up to be delivered.");

            Status = Status.Delivered;
        }

        public void MarkAsCancelled()
        {
            if (Status == Status.Delivered) 
                throw new InvalidOperationException("Order has already been delivered and cannot be cancelled.");
            if (Status == Status.Canceled)
                throw new InvalidOperationException("Order has already been cancelled.");
            if (Status == Status.Picked_Up)
                throw new InvalidOperationException("Order has already been picked up and cannot be cancelled.");
            Status = Status.Canceled;
        }
    }
}
