using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public string ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal Discount { get; }
    public decimal TotalAmount => (UnitPrice * Quantity) - Discount;

    public SaleItem(string productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = CalculateDiscount(quantity, unitPrice);
    }

    private decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity < 4) return 0;
        if (quantity >= 4 && quantity < 10) return unitPrice * quantity * 0.10m;
        if (quantity >= 10 && quantity <= 20) return unitPrice * quantity * 0.20m;

        throw new DomainException("Invalid quantity for discount calculation.");
    }
}
