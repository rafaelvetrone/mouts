using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

/// <summary>
/// Implementation of ISaleRepository using Entity Framework Core
/// </summary>
public class SaleRepository : ISaleRepository
{
    private readonly DefaultContext _context;

    /// <summary>
    /// Initializes a new instance of UserRepository
    /// </summary>
    /// <param name="context">The database context</param>
    public SaleRepository(DefaultContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates a new sale in the database
    /// </summary>
    /// <param name="sale">The sale to create</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale</returns>
    public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        await _context.Sales.AddAsync(sale, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return sale;
    }

    /// <summary>
    /// Retrieves a sale by their unique identifier
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    /// <summary>
    /// Retrieves a sale by its sale number
    /// </summary>
    /// <param name="email">The sale number to search for</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale if found, null otherwise</returns>
    public async Task<Sale?> GetBySaleNumberAsync(string saleNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
    }

    /// <summary>
    /// Deletes a sale from the database
    /// </summary>
    /// <param name="id">The unique identifier of the sale to delete</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the sale was deleted, false if not found</returns>
    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sale = await GetByIdAsync(id, cancellationToken);

        if (sale == null) return false;

        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<Sale> UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        // Load existing Sale with its items
        var existingSale = await _context.Sales
            .Include(s => s.Items)
            .FirstOrDefaultAsync(s => s.Id == sale.Id, cancellationToken);

        if (existingSale == null)
            throw new KeyNotFoundException($"Sale with ID {sale.Id} not found");

        // Update scalar properties
        existingSale.UpdateDetails(sale.SaleNumber, sale.SaleDate, sale.CustomerId,
                           sale.CustomerName, sale.CustomerEmail, sale.Branch, sale.IsCancelled);

        // Remove items that were deleted
        foreach (var dbItem in existingSale.Items.ToList())
        {
            if (!sale.Items.Any(i => i.ProductId == dbItem.ProductId))
                existingSale.RemoveItem(dbItem.Id);
        }

        // Add new items or update existing ones
        foreach (var item in sale.Items)
        {
            existingSale.UpdateItem(item);
        }

        _context.Sales.Update(existingSale); // EF tracks changes automatically

        await _context.SaveChangesAsync(cancellationToken);
        return existingSale;
    }

    public async Task<List<Sale>> ListAsync(SaleFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.Include(s => s.Items).AsQueryable();

        // Filtering
        query = ApplyFilter(query, filter);

        // Ordering
        if (!string.IsNullOrWhiteSpace(filter.Order))
            query = query.OrderBy(filter.Order); // Using System.Linq.Dynamic.Core

        // Pagination
        int skip = (filter.Page - 1) * filter.Size;
        query = query.Skip(skip).Take(filter.Size);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(SaleFilter filter, CancellationToken cancellationToken = default)
    {
        var query = _context.Sales.Include(s => s.Items).AsQueryable();

        // Filtering
        query = ApplyFilter(query, filter);

        // Ordering
        if (!string.IsNullOrWhiteSpace(filter.Order))
            query = query.OrderBy(filter.Order); // Using System.Linq.Dynamic.Core

        // Pagination
        int skip = (filter.Page - 1) * filter.Size;
        query = query.Skip(skip).Take(filter.Size);

        return await query.CountAsync(cancellationToken);
    }

    private IQueryable<Sale> ApplyFilter(IQueryable<Sale> query, SaleFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.SaleNumber))
            query = query.Where(s => s.SaleNumber == filter.SaleNumber);

        if (!string.IsNullOrWhiteSpace(filter.CustomerName))
            query = query.Where(s => s.CustomerName == filter.CustomerName);

        if (!string.IsNullOrWhiteSpace(filter.Branch))
            query = query.Where(s => s.Branch == filter.Branch);

        if (filter.InitialDate.HasValue)
            query = query.Where(s => s.SaleDate >= filter.InitialDate);

        if (filter.EndDate.HasValue)
            query = query.Where(s => s.SaleDate <= filter.EndDate);
        
        return query;
    }

}
