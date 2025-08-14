
using Ambev.DeveloperEvaluation.Application.Utils;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleUpdatedEventHandler : INotificationHandler<SaleUpdatedEvent>
{    
    private readonly IKafkaProducer _kafkaProducer;

    public SaleUpdatedEventHandler(IKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    public async Task Handle(SaleUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var kafkaMessage = new SaleKafkaMessage
        {
            eventType = Constants.SaleUpdatedEventType,
            data = new
            {
                notification.Sale.Id,
                notification.Sale.SaleNumber,
                notification.Sale.SaleDate,
                notification.Sale.CustomerId,
                notification.Sale.CustomerName,
                notification.Sale.CustomerEmail,
                notification.Sale.Branch
            }
        };

        await _kafkaProducer.ProduceAsync("sales", kafkaMessage, cancellationToken);
    }
}
