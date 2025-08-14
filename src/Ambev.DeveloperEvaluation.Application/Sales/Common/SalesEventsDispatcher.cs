using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

public static class SalesEventsDispatcher
{
    public static async Task DispatchAsync(Sale sale, IMediator mediator, CancellationToken ct)
    {
        foreach (var domainEvent in sale.DomainEvents)
        {
            switch (domainEvent)
            {
                case SaleCancelledDomainEvent cancelled:
                    await mediator.Publish(new SaleCancelledEvent(cancelled.Sale), ct);
                    break;
                case SaleCreatedDomainEvent created:
                    await mediator.Publish(new SaleCreatedEvent(created.Sale), ct);
                    break;
                case SaleUpdatedDomainEvent updated:
                    await mediator.Publish(new SaleUpdatedEvent(updated.Sale), ct);
                    break;
                case ItemCancelledDomainEvent itemCancelled:
                    await mediator.Publish(new ItemCancelledEvent(itemCancelled.SaleItem), ct);
                    break;
            }
        }
        sale.ClearDomainEvents();
    }
}
