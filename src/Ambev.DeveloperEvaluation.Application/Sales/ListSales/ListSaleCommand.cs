
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Command for retrieving a sale by a filter.
/// </summary>
public class ListSaleCommand : IRequest<IEnumerable<GetSaleResult>>
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
