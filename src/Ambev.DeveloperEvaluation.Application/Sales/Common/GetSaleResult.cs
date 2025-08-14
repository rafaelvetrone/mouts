namespace Ambev.DeveloperEvaluation.Application.Sales.Common;

/// <summary>
/// Response model for GetSale operation
/// </summary>
public class GetSaleResult
{
    /// <summary>
    /// The unique identifier of the created user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The unique identifier for a sale transaction.
    /// </summary>
    public string SaleNumber { get; set; } = string.Empty;

    /// <summary>
    /// The date and time when the sale was made. Format : yyyy-MM-ddTHH:mm:ss.fffffffZ
    /// </summary>
    public DateTime SaleDate { get; set; }

    /// <summary>
    /// The unique identifier for the customer associated with the sale.
    /// </summary>
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    /// The name of the customer associated with the sale.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// The email address of the customer associated with the sale.
    /// </summary>
    public string CustomerEmail { get; set; } = string.Empty;

    /// <summary>
    /// The branch identifier where the sale was made.
    /// </summary>
    public string Branch { get; set; } = string.Empty;

    /// <summary>
    /// The total amount of the sale, calculated as the sum of all item prices with the discounts applied.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Whether the sale is cancelled or not.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// The list of items included in the sale. Each item contains product details.
    /// </summary>
    public List<SaleItemResult> Items { get; set; } = new List<SaleItemResult>();
}
