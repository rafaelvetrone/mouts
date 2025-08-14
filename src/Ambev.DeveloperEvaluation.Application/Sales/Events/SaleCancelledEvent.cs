using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;


namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleCancelledEvent : INotification
{
    public Sale Sale { get; }

    public SaleCancelledEvent(Sale sale)
    {
        Sale = sale;
    }
}