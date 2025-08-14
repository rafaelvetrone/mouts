using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.Common;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

/// <summary>
/// Profile for mapping between Sale entity and UpdateSaleResponse
/// </summary>
public class UpdateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for UpdateSale operation
    /// </summary>
    public UpdateSaleProfile()
    {
        CreateMap<Sale, UpdateSaleResult>();
        CreateMap<SaleItem, SaleItemResult>();

        CreateMap<UpdateSaleCommand, Sale>()
           .ConstructUsing(cmd => new Sale(cmd.SaleNumber, cmd.SaleDate, cmd.CustomerId, cmd.CustomerName, cmd.CustomerEmail, cmd.Branch))
           .ForMember(dest => dest.Items, opt => opt.Ignore());
    }
}
