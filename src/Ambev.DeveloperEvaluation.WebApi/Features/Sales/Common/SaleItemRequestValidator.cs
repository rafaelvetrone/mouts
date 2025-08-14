using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;

public class SaleItemRequestValidator : AbstractValidator<SaleItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - ProductId: must not be empty
    /// - ProductName: must not be empty
    /// - Quantity: must be greater than 0
    /// - UnitPrice: must be greater than 0
    /// </remarks>
    public SaleItemRequestValidator()
    {
        RuleFor(item => item.ProductId).NotEmpty();
        RuleFor(item => item.ProductName).NotEmpty();
        RuleFor(item => item.Quantity).GreaterThan(0).WithMessage("Item quantity must be greater than 0");
        RuleFor(item => item.UnitPrice).GreaterThan(0).WithMessage("Item unit price must be greater than 0");
    }
}