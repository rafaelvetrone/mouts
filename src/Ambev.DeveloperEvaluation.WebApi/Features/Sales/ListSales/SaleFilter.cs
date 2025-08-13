namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// filtering and pagination model for listing sales
/// </summary>
public class SaleFilter
{
    // Filtering
    public string? SaleNumber { get; set; }
    public string? Branch { get; set; }
    public string? CustomerName { get; set; }
    public DateTime InitialDate { get; set; }
    public DateTime EndDate { get; set; }


    // Pagination
    public int _page { get; set; } = 1;
    public int _size { get; set; } = 10;

    // Ordering format: "customerName desc, totalAmount asc"
    public string? _order { get; set; }
}
