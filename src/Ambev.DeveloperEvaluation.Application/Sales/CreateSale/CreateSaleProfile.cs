using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and CreateSaleResponse
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));     
            
            CreateMap<Sale, CreateSaleResult>();
        }
    }
}
