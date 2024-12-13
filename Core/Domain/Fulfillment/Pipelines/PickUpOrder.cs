using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;
using SmaHauJenHoaVij.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace SmaHauJenHoaVij.Core.Domain.Fulfillment.Pipelines
{
    public class PickUpOrder
    {
        public record Request(Guid OfferId) : IRequest<Unit>; // Specify Unit explicitly

        public class Handler : IRequestHandler<Request, Unit> // Specify Unit explicitly here
        {
            private readonly ShopContext _db;
            private readonly IMediator _mediator;

            public Handler(ShopContext db, IMediator mediator)
            {
                _db = db;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                // Fetch the offer without using Include
                var offer = await _db.Offers
                    .FirstOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);

                if (offer == null || offer.CourierId == null)
                {
                    throw new Exception("Offer not found or not assigned to a courier.");
                }

                // Fetch the reimbursement separately
                var reimbursement = await _db.Reimbursements
                    .FirstOrDefaultAsync(r => r.Id == offer.Reimbursement, cancellationToken);

                if (reimbursement == null)
                {
                    throw new Exception($"Reimbursement for Offer ID {request.OfferId} not found.");
                }

                // Publish the event
                var notification = new OrderPickedUp(offer.OrderId, offer.CourierId.Value);
                offer.Events.Add(notification);

                // Save changes and publish domain events
                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
