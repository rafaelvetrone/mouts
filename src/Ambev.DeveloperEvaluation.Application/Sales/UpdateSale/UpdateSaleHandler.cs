using Ambev.DeveloperEvaluation.Application.Sales.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Handler for processing CreateSaleCommand requests
/// </summary>
public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UpdateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of UpdateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="mediator">The Logger instance</param>
    /// <param name="logger">The Logger instance</param>    
    public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IMediator mediator, ILogger<UpdateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Handles the UpdateSaleCommand request
    /// </summary>
    /// <param name="command">The CreateSale command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (sale == null)
            throw new KeyNotFoundException($"Sale with id {command.Id} not found");

        // Update basic properties
        sale.UpdateDetails(
            command.SaleNumber,
            command.SaleDate,
            command.CustomerId,
            command.CustomerName,
            command.CustomerEmail,
            command.Branch,
            command.IsCancelled
        );

        // Handle sale items
        var updatedItems = _mapper.Map<List<SaleItem>>(command.Items);

        // Add or update items
        foreach (var item in updatedItems)
        {
            var existingItem = sale.Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Update(item.ProductName, item.Quantity, item.UnitPrice);
            }
            else
            {
                sale.AddItem(item.ProductId, item.ProductName, item.Quantity, item.UnitPrice);
            }
        }

        // Remove items that are no longer in the command
        var itemsToRemove = sale.Items
            .Where(i => !updatedItems.Any(ui => ui.ProductId == i.ProductId))
            .ToList();

        foreach (var item in itemsToRemove)
        {
            sale.RemoveItem(item.Id); // you’d implement RemoveItem in Sale entity
        }

        if (command.IsCancelled)
        {
            sale.Cancel();
        }

        var createdSale = await _saleRepository.UpdateAsync(sale, cancellationToken);

        await SalesEventsDispatcher.DispatchAsync(sale, _mediator, cancellationToken);

        _logger.LogInformation("Sale with number {SaleNumber} updated successfully", createdSale.SaleNumber);

        var result = _mapper.Map<UpdateSaleResult>(createdSale);
        return result;
    }
}
