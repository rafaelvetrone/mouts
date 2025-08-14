using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;


namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class ItemCancelledEvent : INotification
{
    public SaleItem SaleItem { get; }

    public ItemCancelledEvent(SaleItem saleItem)
    {
        SaleItem = saleItem;
    }
}