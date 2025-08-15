using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

public class PagedSalesResponse
{
    public IEnumerable<GetSaleResponse> Items { get; set; } = [];
    public int TotalCount { get; set; }
}
