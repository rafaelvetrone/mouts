using Ambev.DeveloperEvaluation.Application.Sales.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class PagedSalesResult
{
    public IEnumerable<GetSaleResult> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
