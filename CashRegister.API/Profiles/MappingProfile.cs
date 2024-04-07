using AutoMapper;
using CashRegister.Application.Services;
using CashRegister.Application.Services.Dto;
using CashRegister.Domain.Models;

namespace CashRegister.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Bill, BillDto>();
            CreateMap<ProductBill, ProductBillDto>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<CreateUpdateProductRequest, Product>();
        }
    }
}
