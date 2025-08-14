using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class UpdateSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// </summary>
    private static Faker<UpdateSaleCommand> updateSaleHandlerFaker()
    {
        // Faker for SaleItemDto
        var saleItemFaker = new Faker<SaleItemDto>()
            .RuleFor(x => x.ProductId, f => f.Random.Guid())
            .RuleFor(x => x.ProductName, f => f.Commerce.ProductName())
            .RuleFor(x => x.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(x => x.UnitPrice, f => f.Finance.Amount(5, 500));

        // Faker for UpdateSaleCommand
        var saleFaker = new Faker<UpdateSaleCommand>()
            .RuleFor(x => x.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
            .RuleFor(x => x.SaleDate, f => f.Date.Recent())
            .RuleFor(x => x.CustomerId, f => f.Random.Guid().ToString())
            .RuleFor(x => x.CustomerName, f => f.Name.FullName())
            .RuleFor(x => x.CustomerEmail, f => f.Internet.Email())
            .RuleFor(x => x.Branch, f => f.Company.CompanyName())
            .RuleFor(x => x.IsCancelled, f => f.Random.Bool(0.1f)) // 10% chance cancelled
            .RuleFor(x => x.Items, f => saleItemFaker.Generate(f.Random.Int(1, 5))); // 1-5 items

        return saleFaker;
    }

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static UpdateSaleCommand GenerateValidCommand()
    {
        return updateSaleHandlerFaker().Generate();
    }
}