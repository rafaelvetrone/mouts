using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.TestData.Sales;
using Ambev.DeveloperEvaluation.Unit.Common;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="ListSalesHandler"/> class.
/// </summary>
public class ListSalesHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListSalesHandler> _logger;
    private readonly ListSalesHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="ListSalesHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public ListSalesHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _logger = Substitute.For<ILogger<ListSalesHandler>>();
        _handler = new ListSalesHandler(_saleRepository, _mapper, _logger);
    }

    /// <summary>
    /// Tests that a valid sale update request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = ListSaleHandlerTestData.GenerateValidCommand();

        var saleFilter = new SaleFilter();
        var salesFromRepo = new List<Sale> { SaleTestData.GenerateData() };
        var mappedResults = new List<GetSaleResult> { new GetSaleResult() };

        _mapper.Map<SaleFilter>(command).Returns(saleFilter);
        _saleRepository.ListAsync(saleFilter, Arg.Any<CancellationToken>())
            .Returns(salesFromRepo);
        _mapper.Map<IEnumerable<GetSaleResult>>(salesFromRepo)
            .Returns(mappedResults);

        // When
        var listSalesResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Same(mappedResults, listSalesResult);
        await _saleRepository.Received(1).ListAsync(saleFilter, Arg.Any<CancellationToken>());
        _mapper.Received(1).Map<SaleFilter>(command);
        _mapper.Received(1).Map<IEnumerable<GetSaleResult>>(salesFromRepo);
    }

    /// <summary>
    /// Tests that an invalid user creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When deleting sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new ListSalesCommand(); // Empty command will fail validation
        command.Page = 0;

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact]
    public async Task Handle_ValidRequest_RepositoryReturnsEmptyList_ReturnsEmptyMappedList()
    {
        // Arrange
        var command = ListSaleHandlerTestData.GenerateValidCommand();

        var saleFilter = new SaleFilter();
        var salesFromRepo = new List<Sale>();
        var mappedResults = new List<GetSaleResult>();

        _mapper.Map<SaleFilter>(command).Returns(saleFilter);
        _saleRepository.ListAsync(saleFilter, Arg.Any<CancellationToken>())
            .Returns(salesFromRepo);
        _mapper.Map<IEnumerable<GetSaleResult>>(salesFromRepo)
            .Returns(mappedResults);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
