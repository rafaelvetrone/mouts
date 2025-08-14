namespace Ambev.DeveloperEvaluation.Application.Sales.Events;

public class SaleKafkaMessage
{
    public string eventType { get; set; }
    public object data { get; set; }
}
