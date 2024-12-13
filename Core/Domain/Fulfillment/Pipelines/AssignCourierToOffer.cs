using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SmaHauJenHoaVij.Infrastructure.Data;
using SmaHauJenHoaVij.Core.Domain.Fulfillment.Events;

namespace Core.Domain.Fulfillment.Pipelines
{
    public class AssignCourierToOffer
    {
        public record Request(Guid OfferId, Guid CourierId) : IRequest<Unit>; // Explicitly specify Unit as the return type

        public class Handler : IRequestHandler<Request, Unit> // Properly implement IRequestHandler
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
                // Get the offer
                var offer = await _db.Offers
                    .FirstOrDefaultAsync(o => o.Id == request.OfferId, cancellationToken);

                if (offer == null)
                {
                    throw new Exception($"Offer with ID {request.OfferId} not found.");
                }

                // Fetch the reimbursement separately
                var reimbursement = await _db.Reimbursements
                    .FirstOrDefaultAsync(r => r.Id == offer.Reimbursement, cancellationToken);

                if (reimbursement == null)
                {
                    throw new Exception($"Reimbursement for Offer ID {request.OfferId} not found.");
                }

                // Assign the courier to the offer and the reimbursement
                offer.AssignCourier(request.CourierId);
                reimbursement.AssignCourier(request.CourierId);

                // Trigger event to mark order as accepted
                var notification = new OfferCourierSet(offer.OrderId, request.CourierId);
                offer.Events.Add(notification);

                await _db.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
