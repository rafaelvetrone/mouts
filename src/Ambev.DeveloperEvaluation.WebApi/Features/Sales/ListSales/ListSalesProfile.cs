using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Profile for mapping GetSale feature requests to commands
/// </summary>
public class ListSaleProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for ListSales feature
    /// </summary>
    public ListSaleProfile()
    {
        CreateMap<SaleFilterRequest, ListSalesCommand>()
            .ForMember(dest => dest.Page, opt => opt.MapFrom(src => src._page))
            .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src._size))
            .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src._order));
    }
}
