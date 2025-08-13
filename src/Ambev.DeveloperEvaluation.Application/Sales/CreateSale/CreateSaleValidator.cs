using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Validator for CreateSaleCommand that defines validation rules for sale creation command.
/// </summary>
public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    /// <summary>
    /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// - SaleNumber: must not be empty
    /// - CustomerId: must not be empty
    /// - CustomerName: must not be empty
    /// - CustomerEmail: Must be valid format (using EmailValidator)
    /// - Branch: must not be empty
    /// - Items: Each item must be validated using SaleItemRequestValidator
    /// </remarks>
    public CreateSaleCommandValidator()
    {
        RuleFor(sale => sale.SaleNumber).NotEmpty();
        RuleFor(sale => sale.CustomerId).NotEmpty();
        RuleFor(sale => sale.CustomerName).NotEmpty();
        RuleFor(sale => sale.CustomerEmail).SetValidator(new EmailValidator());
        RuleFor(sale => sale.Branch).NotEmpty();
        RuleForEach(sale => sale.Items).SetValidator(new SaleItemDtoValidator());
    }

    public class SaleItemDtoValidator : AbstractValidator<SaleItemDto>
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
        public SaleItemDtoValidator()
        {
            RuleFor(item => item.ProductId).NotEmpty();
            RuleFor(item => item.ProductName).NotEmpty();
            RuleFor(item => item.Quantity).GreaterThan(0).WithMessage("Item quantity must be greater than 0");
            RuleFor(item => item.UnitPrice).GreaterThan(0).WithMessage("Item unit price must be greater than 0");
        }
    }
}