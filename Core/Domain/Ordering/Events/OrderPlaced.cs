
using System;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Events;

    public record OrderPlaced : BaseDomainEvent
    {
        public Guid OrderId { get; private set; }
        public Guid CustomerId { get; private set; }
        public string CustomerName { get; private set; }
        public decimal TotalAmount { get; private set; }

        
        public OrderPlaced(Guid orderId, Guid customerId, string customerName, decimal totalAmount)
        {
            OrderId = orderId;
            CustomerName = customerName;
            TotalAmount = totalAmount;
            CustomerId = customerId;

        }
    }

