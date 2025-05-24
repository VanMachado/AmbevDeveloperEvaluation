using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleProfile : Profile
    {
        public UpdateSaleProfile()
        {
            CreateMap<UpdateSaleRequest, UpdateSaleCommand>()
                 .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate))
                 .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<UpdateSaleResult, UpdateSaleResponse>();
            CreateMap<UpdateSaleitemRequest, UpdateSaleItemCommand>();
        }
    }
}
