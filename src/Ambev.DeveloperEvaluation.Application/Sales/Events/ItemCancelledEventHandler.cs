
using Ambev.DeveloperEvaluation.Application.Utils;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class ItemCancelledEventHandler : INotificationHandler<ItemCancelledEvent>
{
    private readonly IKafkaProducer _kafkaProducer;

    public ItemCancelledEventHandler(IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task Handle(ItemCancelledEvent notification, CancellationToken cancellationToken)
    {
        var kafkaMessage = new SaleKafkaMessage
        {
            eventType = Constants.ItemCancelledEventType,
            data = new 
            { 
                notification.SaleItem.SaleId,
                notification.SaleItem.ProductId, 
                notification.SaleItem.Quantity, 
                notification.SaleItem.UnitPrice, 
                notification.SaleItem.ProductName
            }
        };

        await _kafkaProducer.ProduceAsync("sales", kafkaMessage, cancellationToken);
    }
}
