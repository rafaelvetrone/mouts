namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// API Response model for an item in a sale, including product details and quantity.
/// </summary>
public class SaleItemResponse
{
    /// <summary>
    /// The unique identifier for the product associated with the sale item.
    /// </summary>
    public string ProductId { get; set; } = string.Empty;

    /// <summary>
    /// The name of the product associated with the sale item.
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// The quantity of the product sold in this sale item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// The unit price of the product in this sale item.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// The total price for this sale item, calculated as Quantity * UnitPrice - Discount.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Discount applied to this sale item, if any. It depends on the business rules.
    /// </summary>
    public decimal Discount { get; set; }
}
