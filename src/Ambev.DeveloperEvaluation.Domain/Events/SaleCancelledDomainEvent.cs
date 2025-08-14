using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledDomainEvent : IDomainEvent
{
    public Sale Sale { get; }

    public SaleCancelledDomainEvent(Sale sale)
    {
        Sale = sale;
    }
}
