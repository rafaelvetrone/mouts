namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleFilter
{
    // Filtering
    public string? SaleNumber { get; set; }
    public string? Branch { get; set; }
    public string? CustomerName { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Pagination
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

    // Ordering
    public string? Order { get; set; }
}
