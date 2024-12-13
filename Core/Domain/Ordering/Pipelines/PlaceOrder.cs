using MediatR;
using System;
using System.Collections.Generic;
using SmaHauJenHoaVij.Core.Domain.Ordering.Dtos;

namespace SmaHauJenHoaVij.Core.Domain.Ordering.Pipelines
{
    public class PlaceOrder
    {
        public record Request(
            Guid CustomerId, // Added CustomerId
            string CustomerName,
            LocationDto Location,
            List<OrderLineDto> OrderLines
        ) : IRequest<Response>;

        public record Response(
            Guid OrderId, // Changed to Guid
            bool Success,
            string Message
        );
    }
}
