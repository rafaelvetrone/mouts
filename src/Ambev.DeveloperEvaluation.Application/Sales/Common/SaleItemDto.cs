namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

/// <summary>
/// Represents an item in a sale, including product details and quantity.
/// </summary>
public class SaleItemDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the product associated with the sale item.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the name of the product associated with the sale item.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the quantity of the product sold in this sale item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product in this sale item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the total price of the product in this sale item.
    /// </summary>
    public decimal TotalPrice { get; set; }
}