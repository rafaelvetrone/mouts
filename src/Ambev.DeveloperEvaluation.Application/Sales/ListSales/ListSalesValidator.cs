using FluentValidation;
using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Validator for GetSaleCommand
/// </summary>
public class ListSalesValidator : AbstractValidator<ListSalesCommand>
{
    /// <summary>
    /// Initializes validation rules for ListSalesCommand
    /// </summary>
    public ListSalesValidator()
    {
        // Date range validation
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.InitialDate)
            .WithMessage("EndDate must be greater than or equal to InitialDate");

        // Pagination rules
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("_page must be at least 1");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("_size must be between 1 and 100");

        // _order format validation
        RuleFor(x => x.Order)
            .Matches(new Regex(@"^(\s*\w+(\s+(asc|desc))?\s*)(,\s*\w+(\s+(asc|desc))?\s*)*$", RegexOptions.IgnoreCase))
            .When(x => !string.IsNullOrWhiteSpace(x.Order))
            .WithMessage("_order format is invalid. Example: 'customerName desc, totalAmount asc'");

        // _order property name validation (optional: only allow known fields)
        RuleFor(x => x.Order)
            .Must(BeValidOrderFields)
            .When(x => !string.IsNullOrWhiteSpace(x.Order))
            .WithMessage("_order contains invalid property names.");
    }

    private bool BeValidOrderFields(string orderString)
    {
        var validFields = new[] { "saleNumber", "branch", "customerName", "initialDate", "endDate" };
        var fields = orderString.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(part => part.Trim().Split(' ')[0].ToLower());

        return fields.All(f => validFields.Contains(f));
    }
}
