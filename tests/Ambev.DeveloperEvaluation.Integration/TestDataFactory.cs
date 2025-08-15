using Ambev.DeveloperEvaluation.WebApi.Features.Sales.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Integration;

public static class TestDataFactory
{
    public static CreateSaleRequest CreateSampleSaleRequest()
    {
        return new CreateSaleRequest
        {
            SaleNumber = "SALE-" + Guid.NewGuid(),
            SaleDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid().ToString(),
            CustomerName = "John Doe",
            CustomerEmail = "john.doe@example.com",
            Branch = "NYC-001",
            IsCancelled = false,
            Items = new List<SaleItemRequest>
                {
                    new SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Coca-Cola 500ml",
                        Quantity = 2,
                        UnitPrice = 1.99m
                    },
                    new SaleItemRequest
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Lays Classic Chips 150g",
                        Quantity = 1,
                        UnitPrice = 2.49m
                    }
                }
        };
    }
}