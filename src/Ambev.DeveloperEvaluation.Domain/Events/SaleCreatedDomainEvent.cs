using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedDomainEvent: IDomainEvent
{
    public Sale Sale { get; }

    public SaleCreatedDomainEvent(Sale sale)
    {
        Sale = sale;
    }
}
