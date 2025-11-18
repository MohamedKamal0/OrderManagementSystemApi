using AutoMapper;
using OrderManagementSystemApplication.Dtos.Product;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductResponseDto>();
            // CreateMap<ProductUpdateDto, Product>();

        }
    }
}