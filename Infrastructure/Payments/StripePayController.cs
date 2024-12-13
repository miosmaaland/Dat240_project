using Microsoft.AspNetCore.Mvc;
using SmaHauJenHoaVij.Infrastructure.Data;
using Stripe.Checkout;
using System;
using System.Linq;

[Route("create-checkout-session")]
[ApiController]
public class StripePayController : Controller
{
    private readonly ShopContext _db;

    public StripePayController(ShopContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [HttpPost]
    public ActionResult Create([FromForm] Guid invoiceId, [FromForm] decimal tipAmount)
    {

        Console.WriteLine($"Received InvoiceId in Controller: {invoiceId}, TipAmount: {tipAmount}");
        // Get the authenticated user's CustomerId (ensure session is populated correctly)
        var customerId = Guid.Parse(HttpContext.Session.GetString("UserId"));
        Console.WriteLine($"CustomerId from session: {customerId}, InvoiceId from request: {invoiceId}");

        // Fetch the specific invoice by its unique ID and ensure it belongs to the customer
        var invoice = _db.Invoices
            .SingleOrDefault(i => i.Id == invoiceId && i.CustomerId == customerId);

        if (invoice == null)
        {
            Console.WriteLine($"No invoice found for InvoiceId: {invoiceId} and CustomerId: {customerId}");
            return BadRequest("Invoice not found or does not belong to the current user.");
        }

        // Ensure the invoice status is 'Sent'
        if (invoice.Status != InvoiceStatus.Sent)
        {
            Console.WriteLine($"Invoice found, but status is not 'Sent'. Status: {invoice.Status}");
            return BadRequest("Invoice is not in 'Sent' status.");
        }
        
         invoice.UpdateTip(tipAmount);
        _db.SaveChanges();

        var totalAmount = invoice.TotalAmount;
        // Create a Stripe session for payment

        var domain = "http://localhost:5268";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "nok",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = $"Invoice {invoice.Id}",
                    },
                    UnitAmount = (long)(totalAmount * 100), // Convert to cents
                },
                Quantity = 1,
            },
        },
            Mode = "payment",
            SuccessUrl = $"{domain}/CustomerPages/Payment/success/{invoice.Id}",
            CancelUrl =  $"{domain}/CustomerPages/Payment/Cancelled/{invoice.Id}",
        };

        var service = new SessionService();
        var session = service.Create(options);

        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303); // Redirect to Stripe's checkout
    }

}