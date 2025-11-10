using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderManagementSystemApplication.Dtos.ShoppingCartDto;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class CartMappingProfile:Profile
    {

        public CartMappingProfile() 
        {
            CreateMap<CartItem, ShoppingItemResponseDto>()
              .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null))
              .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));

            // 🧮 Cart -> ShoppingResponseDto
            CreateMap<Cart, ShoppingResponseDto>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ForMember(dest => dest.TotalBasePrice, opt => opt.MapFrom(src =>
                    src.CartItems.Sum(ci => ci.UnitPrice * ci.Quantity)))
                .ForMember(dest => dest.TotalDiscount, opt => opt.MapFrom(src =>
                    src.CartItems.Sum(ci => ci.Discount * ci.Quantity)))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src =>
                    src.CartItems.Sum(ci => ci.TotalPrice)));

        }


    }
}
