using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Common;

public static class SaleTestData
{
    private static Faker<Sale> saleFaker = new Faker<Sale>()
        .CustomInstantiator(f => new Sale(
            saleNumber: f.Random.AlphaNumeric(8),
            saleDate: f.Date.Recent(30, DateTime.UtcNow).Date,
            customerId: Guid.NewGuid().ToString(),
            customerName: f.Name.FullName(),
            customerEmail: f.Internet.Email(),
            branch: f.Company.CompanyName()
        ));

    /// <summary>
    /// Generates a valid Sale entity with randomized data.
    /// The generated user will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Sale entity with randomly generated data.</returns>
    public static Sale GenerateData()
    {
        return saleFaker.Generate();
    }
}