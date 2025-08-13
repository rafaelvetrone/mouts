using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public string CustomerId { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public decimal TotalAmount => Items.Sum(i => i.TotalAmount);
    public string Branch { get; private set; }
    public bool IsCancelled { get; private set; }
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

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
    }

    public void AddItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        var item = new SaleItem(this, productId, productName, quantity, unitPrice);
        _items.Add(item);
    }

    public void Cancel()
    {
        IsCancelled = true;
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
    }

    public void UpdateItem(SaleItem updatedItem)
    {
        var existingItem = _items.FirstOrDefault(i => i.Id == updatedItem.Id);
        if (existingItem != null)
        {
            existingItem.Update(updatedItem.ProductId, updatedItem.ProductName,
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
}
