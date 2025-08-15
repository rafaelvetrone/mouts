using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }

    [NotMapped]
    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
    public string Branch { get; private set; }
    public bool IsCancelled { get; private set; }
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

    private List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    private Sale() { } // EF Core

    public Sale(string saleNumber, DateTime saleDate, string customerId, string customerName, string customerEmail, string branch)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        Branch = branch;
        IsCancelled = false;

        _domainEvents.Add(new SaleCreatedDomainEvent(this));
    }

    public void AddItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        var item = new SaleItem(this, productId, productName, quantity, unitPrice);
        _items.Add(item);
    }

    public void Cancel()
    {
        IsCancelled = true;

        _domainEvents.Add(new SaleCancelledDomainEvent(this));

        foreach (var item in _items)
        {
            _domainEvents.Add(new ItemCancelledDomainEvent(item));
        }
    }

    public void UpdateDetails(string saleNumber, DateTime saleDate, string customerId,
                              string customerName, string customerEmail, string branch, bool isCancelled)
    {
        SaleNumber = saleNumber;
        SaleDate = saleDate;
        CustomerId = customerId;
        CustomerName = customerName;
        CustomerEmail = customerEmail;
        Branch = branch;
        IsCancelled = isCancelled;

        _domainEvents.Add(new SaleUpdatedDomainEvent(this));
    }

    public void UpdateItem(SaleItem updatedItem)
    {
        var existingItem = _items.FirstOrDefault(i => i.ProductId == updatedItem.ProductId);
        if (existingItem != null)
        {
            existingItem.Update(updatedItem.ProductName,
                                updatedItem.Quantity, updatedItem.UnitPrice);
        }
        else
        {
            AddItem(updatedItem.ProductId, updatedItem.ProductName, updatedItem.Quantity, updatedItem.UnitPrice);
        }
    }

    public void RemoveItem(Guid itemId)
    {
        var item = _items.FirstOrDefault(i => i.Id == itemId);
        if (item != null)
            _items.Remove(item);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
