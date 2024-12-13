using System;
using SmaHauJenHoaVij.SharedKernel;

namespace SmaHauJenHoaVij.Core.Domain.Invoicing;

public class Invoice : BaseEntity
{
    private Invoice() { } // EF Core

    public Invoice(Guid customerId, Guid orderId, decimal amount, decimal tip = 0)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        OrderId = orderId;
        Amount = amount;
        Status = InvoiceStatus.New;
        Tip = tip;
    }

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public decimal Tip { get; private set; }
    public InvoiceStatus Status { get; private set; }

    public decimal TotalAmount => Amount + Tip;

    public void MarkAsSent()
    {
        if (Status != InvoiceStatus.New)
            throw new InvalidOperationException("Invoice must be in 'New' status to mark as sent.");
        Status = InvoiceStatus.Sent;
    }

    public void MarkAsPaid()
    {
        if (Status != InvoiceStatus.Sent)
            throw new InvalidOperationException("Invoice must be in 'Sent' status to mark as paid.");
        Status = InvoiceStatus.Paid;
    }

    public void MarkAsCanceled()
    {
        if (Status == InvoiceStatus.Paid)
            throw new InvalidOperationException("Cannot cancel a paid invoice.");
        Status = InvoiceStatus.Cancelled;
    }

    public void UpdateTip(decimal newTip)
    {
        if (newTip < 0)
        {
            throw new InvalidOperationException("Tip cannot be negative.");
        }

        Tip = newTip;
    }

}