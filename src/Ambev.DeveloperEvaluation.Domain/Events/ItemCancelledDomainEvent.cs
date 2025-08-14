using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class ItemCancelledDomainEvent : IDomainEvent
{
    public SaleItem SaleItem { get; }

    public ItemCancelledDomainEvent(SaleItem saleItem)
    {
        SaleItem = saleItem;
    }
}
