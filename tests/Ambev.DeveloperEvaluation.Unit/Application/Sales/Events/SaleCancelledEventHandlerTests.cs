using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Application.Utils;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.Unit.Common;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.Events;

public class SaleCancelledEventHandlerTests
{
    [Fact]
    public async Task Handle_ShouldPublishSaleCreatedMessageToKafka()
    {
        // Arrange
        var kafkaProducer = Substitute.For<IKafkaProducer>();
        var handler = new SaleCancelledEventHandler(kafkaProducer);

        var sale = SaleTestData.GenerateData();

        var notification = new SaleCancelledEvent(sale);
        var cancellationToken = CancellationToken.None;

        // Act
        await handler.Handle(notification, cancellationToken);

        // Assert
        await kafkaProducer.Received(1)
            .ProduceAsync("sales",
                          Arg.Is<SaleKafkaMessage>(m =>
                              m.eventType == Constants.SaleCancelledEventType),
                          cancellationToken);
    }
}