using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

/// <summary>
/// Handler for processing GetSaleCommand requests
/// </summary>
public class ListSalesHandler : IRequestHandler<ListSalesCommand, PagedSalesResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListSalesHandler> _logger;

    /// <summary>
    /// Initializes a new instance of GetSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="_logger">The Logger instance</param>
    public ListSalesHandler(
        ISaleRepository saleRepository,
        IMapper mapper,
        ILogger<ListSalesHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the GetSaleCommand request
    /// </summary>
    /// <param name="request">The GetSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    public async Task<PagedSalesResult> Handle(ListSalesCommand request, CancellationToken cancellationToken)
    {
        var validator = new ListSalesValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var saleFilter = _mapper.Map<SaleFilter>(request);

        var sales = await _saleRepository.ListAsync(saleFilter, cancellationToken);

        // Get total count ignoring pagination
        var totalCount = await _saleRepository.CountAsync(saleFilter, cancellationToken);

        _logger.LogInformation("Returning Sales with filters {@request}", request);

        return new PagedSalesResult
        {
            Items = _mapper.Map<IEnumerable<GetSaleResult>>(sales),
            TotalCount = totalCount
        };
    }
}
