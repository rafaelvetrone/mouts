using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Unit.Common;
using Bogus;
using System.Reflection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{    
    [Fact]
    public void Ctor_ShouldInitializeFields_AndRaiseCreatedEvent()
    {
        // Act
        var sale = SaleTestData.GenerateData();

        // Assert basics
        Assert.False(sale.IsCancelled);
        Assert.False(string.IsNullOrWhiteSpace(sale.SaleNumber));
        Assert.False(string.IsNullOrWhiteSpace(sale.Branch));
        Assert.False(string.IsNullOrWhiteSpace(sale.CustomerName));
        Assert.False(string.IsNullOrWhiteSpace(sale.CustomerEmail));
        Assert.NotEqual(Guid.Empty, sale.Id);

        // Created event
        Assert.Contains(sale.DomainEvents, e => e is SaleCreatedDomainEvent);
        Assert.Equal(1, sale.DomainEvents.Count);
    }

    [Fact]
    public void AddItem_ShouldAddItem_AndAffectTotal()
    {
        var sale = SaleTestData.GenerateData();

        // Act
        sale.AddItem(Guid.NewGuid(), "Product A", quantity: 2, unitPrice: 10m);

        // Assert
        Assert.Single(sale.Items);
        var item = sale.Items.Single();
        Assert.Equal("Product A", item.ProductName);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(10m, item.UnitPrice);

        // Assuming TotalAmount sums item.TotalAmount (no discount for < 4)
        Assert.Equal(20m, sale.TotalAmount);
    }

    [Fact]
    public void TotalAmount_ShouldSumAllItems()
    {
        var sale = SaleTestData.GenerateData();

        sale.AddItem(Guid.NewGuid(), "A", 2, 10m); // 20
        sale.AddItem(Guid.NewGuid(), "B", 3, 20m); // 60

        Assert.Equal(80m, sale.TotalAmount);
    }

    [Fact]
    public void Cancel_ShouldSetIsCancelled_AndRaiseSaleAndItemEvents()
    {
        var sale = SaleTestData.GenerateData();
        sale.AddItem(Guid.NewGuid(), "A", 1, 10m);
        sale.AddItem(Guid.NewGuid(), "B", 2, 5m);

        // Created event already present
        Assert.Equal(1, sale.DomainEvents.Count);

        // Act
        sale.Cancel();

        // Assert state
        Assert.True(sale.IsCancelled);

        // Domain events: 1 (created) + 1 (sale cancelled) + 2 (items cancelled)
        Assert.Equal(1 + 1 + 2, sale.DomainEvents.Count);
        Assert.Single(sale.DomainEvents.OfType<SaleCancelledDomainEvent>());
        Assert.Equal(2, sale.DomainEvents.OfType<ItemCancelledDomainEvent>().Count());
    }

    [Fact]
    public void UpdateDetails_ShouldOverwriteFields_AndRaiseUpdatedEvent()
    {
        var sale = SaleTestData.GenerateData();

        var newDate = DateTime.UtcNow.Date.AddDays(-1);
        sale.UpdateDetails(
            saleNumber: "S-002",
            saleDate: newDate,
            customerId: "CUST-2",
            customerName: "Bob Smith",
            customerEmail: "bob@example.com",
            branch: "Store 2",
            isCancelled: true);

        Assert.Equal("S-002", sale.SaleNumber);
        Assert.Equal(newDate, sale.SaleDate);
        Assert.Equal("CUST-2", sale.CustomerId);
        Assert.Equal("Bob Smith", sale.CustomerName);
        Assert.Equal("bob@example.com", sale.CustomerEmail);
        Assert.Equal("Store 2", sale.Branch);
        Assert.True(sale.IsCancelled);

        Assert.Contains(sale.DomainEvents, e => e is SaleUpdatedDomainEvent);
    }

    [Fact]
    public void UpdateItem_WhenItemExists_ShouldModifyExisting_NotAddNew()
    {
        var sale = SaleTestData.GenerateData();
        sale.AddItem(Guid.NewGuid(), "Original", 2, 10m);
        var existing = sale.Items.Single();

        // Build an updated item instance with the SAME Id (use reflection to set Id)
        var updated = new SaleItem(sale, existing.ProductId, "Updated", 5, 12m);
        updated.Id = existing.Id;

        // Act
        sale.UpdateItem(updated);

        // Assert still one item, but values changed
        Assert.Single(sale.Items);
        var after = sale.Items.Single();
        Assert.Equal(existing.Id, after.Id);
        Assert.Equal("Updated", after.ProductName);
        Assert.Equal(5, after.Quantity);
        Assert.Equal(12m, after.UnitPrice);
    }

    [Fact]
    public void UpdateItem_WhenItemDoesNotExist_ShouldAddNewItem()
    {
        var sale = SaleTestData.GenerateData();
        sale.AddItem(Guid.NewGuid(), "A", 1, 10m);
        var initialCount = sale.Items.Count;

        // New item (different Id)
        var updated = new SaleItem(sale, Guid.NewGuid(), "B", 3, 7m);
        // (Do not align Ids)

        // Act
        sale.UpdateItem(updated);

        // Assert count increased
        Assert.Equal(initialCount + 1, sale.Items.Count);
        Assert.Contains(sale.Items, i => i.ProductName == "B" && i.Quantity == 3 && i.UnitPrice == 7m);
    }

    [Fact]
    public void RemoveItem_ShouldRemoveById_WhenExists()
    {
        var sale = SaleTestData.GenerateData();
        sale.AddItem(Guid.NewGuid(), "A", 1, 10m);
        var item = sale.Items.Single();

        // Act
        sale.RemoveItem(item.Id);

        // Assert
        Assert.Empty(sale.Items);
    }

    [Fact]
    public void RemoveItem_ShouldDoNothing_WhenNotFound()
    {
        var sale = SaleTestData.GenerateData();
        sale.AddItem(Guid.NewGuid(), "A", 1, 10m);

        // Act (random id)
        sale.RemoveItem(Guid.NewGuid());

        // Assert no change
        Assert.Single(sale.Items);
    }

    [Fact]
    public void ClearDomainEvents_ShouldRemoveAllEvents()
    {
        var sale = SaleTestData.GenerateData(); //adds domain event
        sale.Cancel(); // adds more

        Assert.True(sale.DomainEvents.Count > 0);

        // Act
        sale.ClearDomainEvents();

        // Assert
        Assert.Empty(sale.DomainEvents);
    }
}