using System.Collections.Generic;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Dtos
{
    public record OrderDto(
        Guid OrderId, // Changed to Guid
        Guid CustomerId, // Added to link the order to the customer
        string CustomerName,
        LocationDto Location,
        List<OrderLineDto> OrderLines,
        string Status
    );
}
