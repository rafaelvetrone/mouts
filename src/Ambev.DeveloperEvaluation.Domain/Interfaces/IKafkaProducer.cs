using System.Threading;

namespace Ambev.DeveloperEvaluation.Domain.Interfaces;

public interface IKafkaProducer
{
    Task ProduceAsync<T>(string topic, T message, CancellationToken cancellationToken);
}


