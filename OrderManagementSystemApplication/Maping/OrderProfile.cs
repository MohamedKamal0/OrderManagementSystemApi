using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderManagementSystemApplication.Dtos.Order;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderCreateDto, Order>()
                  .ForMember(dest => dest.Id, opt => opt.Ignore())
                  .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
                  .ForMember(dest => dest.OrderDate, opt => opt.Ignore())
                  .ForMember(dest => dest.TotalBaseAmount, opt => opt.Ignore())
                  .ForMember(dest => dest.TotalDiscountAmount, opt => opt.Ignore())
                  .ForMember(dest => dest.ShippingCost, opt => opt.Ignore())
                  .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                  .ForMember(dest => dest.OrderStatus, opt => opt.Ignore())
                  .ForMember(dest => dest.OrderItems, opt => opt.Ignore())
                  .ForMember(dest => dest.Customer, opt => opt.Ignore())
                  .ForMember(dest => dest.BillingAddress, opt => opt.Ignore())
                  .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore());

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => Math.Round(src.TotalAmount, 2)))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            // OrderItem Mappings
            CreateMap<OrderItem, OrderItemResponseDto>();

            CreateMap<OrderItemCreateDto, OrderItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Discount, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());
        
    }
    }
}
