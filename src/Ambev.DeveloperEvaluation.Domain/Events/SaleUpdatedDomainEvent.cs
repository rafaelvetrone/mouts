using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleUpdatedDomainEvent : IDomainEvent
{
    public Sale Sale { get; }

    public SaleUpdatedDomainEvent(Sale sale)
    {
        Sale = sale;
    }
}
