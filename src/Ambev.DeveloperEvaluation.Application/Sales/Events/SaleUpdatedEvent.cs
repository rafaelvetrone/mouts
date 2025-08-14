using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;


namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleUpdatedEvent : INotification
{
    public Sale Sale { get; }

    public SaleUpdatedEvent(Sale sale)
    {
        Sale = sale;
    }
}