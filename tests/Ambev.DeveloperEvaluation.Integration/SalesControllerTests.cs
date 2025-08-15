using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Application.Utils;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using FluentAssertions;
using NSubstitute;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration;

public class SalesControllerTests : IClassFixture<SalesWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly IKafkaProducer _kafkaMock;

    public SalesControllerTests(SalesWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _kafkaMock = factory.KafkaProducerMock;
    }

    [Fact]
    public async Task CreateSale_ShouldPublishKafkaMessage()
    {
        // Arrange
        var request = TestDataFactory.CreateSampleSaleRequest();

        // Act
        var response = await _client.PostAsJsonAsync("/api/sales", request);

        // Assert HTTP response
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert Kafka publish
        await _kafkaMock.Received(1).ProduceAsync(
            "sales",
            Arg.Is<SaleKafkaMessage>(m =>
            m.eventType == Constants.SaleCreatedEventType),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task DeleteSale_ShouldPublishCancelledKafkaMessage()
    {
        // First create a sale
        var createRequest = TestDataFactory.CreateSampleSaleRequest();

        var createResponse = await _client.PostAsJsonAsync("/api/sales", createRequest);
        var content = await createResponse.Content.ReadAsStringAsync();
        Console.WriteLine(content);

        var createdSale = await createResponse.Content.ReadFromJsonAsync<ApiResponseWithData<CreateSaleResponse>>();

        // Act: delete sale
        var response = await _client.DeleteAsync($"/api/sales/{createdSale!.Data.Id}");

        content = await response.Content.ReadAsStringAsync();
        Console.WriteLine(content);

        // Assert HTTP response
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // Assert Kafka publish
        await _kafkaMock.Received(1).ProduceAsync(
            "sales",
            Arg.Is<SaleKafkaMessage>(m =>
            m.eventType == Constants.SaleCancelledEventType),
            Arg.Any<CancellationToken>());
    }
}
