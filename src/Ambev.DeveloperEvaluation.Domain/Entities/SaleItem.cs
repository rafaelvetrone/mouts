using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public string ProductId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }

    public Guid SaleId { get; private set; } // FK to Sale
    public Sale Sale { get; private set; }    // Navigation property

    public SaleItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = CalculateDiscount(quantity, unitPrice);
        TotalAmount = (unitPrice * quantity) - Discount;
    }

    public SaleItem(Sale sale, string productId, string productName, int quantity, decimal unitPrice)
    : this(productId, productName, quantity, unitPrice)
    {
        Sale = sale ?? throw new ArgumentNullException(nameof(sale));
        SaleId = sale.Id;
    }

    private decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity < 4) return 0;
        if (quantity >= 4 && quantity < 10) return unitPrice * quantity * 0.10m;
        if (quantity >= 10 && quantity <= 20) return unitPrice * quantity * 0.20m;

        throw new DomainException("Invalid quantity for discount calculation.");
    }

    public void Update(string productId, string productName, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
