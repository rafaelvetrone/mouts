using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class ListSaleHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid Sale entities.
    /// </summary>
    private static Faker<ListSalesCommand> listSaleHandlerFaker()
    {
        var listSalesCommandFaker = new Faker<ListSalesCommand>()
            .RuleFor(x => x.SaleNumber, f => f.Commerce.Ean13())
            .RuleFor(x => x.Branch, f => f.Company.CompanyName())
            .RuleFor(x => x.CustomerName, f => f.Person.FullName)
            .RuleFor(x => x.InitialDate, f => (DateTime?)f.Date.Past(1))
            .RuleFor(x => x.EndDate, (f, x) => (DateTime?)f.Date.Between(x.InitialDate ?? DateTime.UtcNow.AddYears(-1), DateTime.UtcNow))
            .RuleFor(x => x.Page, f => f.Random.Int(1, 50))
            .RuleFor(x => x.Size, f => f.Random.Int(1, 100))
            .RuleFor(x => x.Order, f => "customerName desc, saleDate asc");

        return listSalesCommandFaker;
    }

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static ListSalesCommand GenerateValidCommand()
    {
        return listSaleHandlerFaker().Generate();
    }
}