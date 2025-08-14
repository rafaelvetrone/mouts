using Ambev.DeveloperEvaluation.Application.Sales.Common;
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
}