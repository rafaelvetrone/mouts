using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Profile for mapping between Sale entity and CreateSaleResponse
/// </summary>
public class CreateSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for CreateUser operation
    /// </summary>
    public CreateSaleProfile()
    {
        //CreateMap<CreateSaleCommand, Sale>();
        //CreateMap<SaleItemDto, SaleItem>();
        CreateMap<Sale, CreateSaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
        
         CreateMap<CreateSaleCommand, Sale>()
            .ConstructUsing(cmd => new Sale(cmd.SaleNumber, cmd.SaleDate, cmd.CustomerId, cmd.CustomerName, cmd.CustomerEmail, cmd.Branch))
            .ForMember(dest => dest.Items, opt => opt.Ignore());

        CreateMap<SaleItemDto, SaleItem>()
            .ConstructUsing(src => new SaleItem(src.ProductId, src.ProductName, src.Quantity, src.UnitPrice));
    }
}
