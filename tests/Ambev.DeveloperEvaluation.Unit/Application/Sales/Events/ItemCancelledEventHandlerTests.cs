using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Application.Utils;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Events;


public class ItemCancelledEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPublishCorrectKafkaMessage()
    {
        // Arrange
        var kafkaProducer = Substitute.For<IKafkaProducer>();
        var handler = new ItemCancelledEventHandler(kafkaProducer);

        var sale = new Sale(
            saleNumber: "S-001",
            saleDate: DateTime.UtcNow,
            customerId: Guid.NewGuid().ToString(),
            customerName: "Test Customer",
            customerEmail: "customer@test.com",
            branch: "Main Branch"
            );
        sale.Id = Guid.NewGuid(); // Assign a new GUID for the sale ID

        var saleItem = new SaleItem(
            sale: sale, // You can pass a fake sale here
            productId: Guid.NewGuid(),
            productName: "Test Product",
            quantity: 2,
            unitPrice: 50m);

        var notification = new ItemCancelledEvent(saleItem);
        var cancellationToken = CancellationToken.None;

        // Act
        await handler.Handle(notification, cancellationToken);

        // Assert
        await kafkaProducer.Received(1).ProduceAsync(
            "sales",
            Arg.Is<SaleKafkaMessage>(msg =>
                msg.eventType == Constants.ItemCancelledEventType),
            cancellationToken);
    }
}
