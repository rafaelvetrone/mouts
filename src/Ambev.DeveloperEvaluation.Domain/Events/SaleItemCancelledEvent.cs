using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleItemCancelledEvent : INotification
{
    public SaleItem SaleItem { get; }

    public SaleItemCancelledEvent(SaleItem saleItem)
    {
        SaleItem = saleItem;
    }
}
