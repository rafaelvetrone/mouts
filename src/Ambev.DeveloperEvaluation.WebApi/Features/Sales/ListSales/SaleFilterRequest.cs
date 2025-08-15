namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// filtering and pagination model for listing sales
/// </summary>
public class SaleFilterRequest
{
    // Filtering
    public string? saleNumber { get; set; }
    public string? branch { get; set; }
    public string? customerName { get; set; }
    public DateTime? initialDate { get; set; }
    public DateTime? endDate { get; set; }


    // Pagination
    public int _page { get; set; } = 1;
    public int _size { get; set; } = 10;

    // Ordering format: "customerName desc, totalAmount asc"
    public string? _order { get; set; }
}
