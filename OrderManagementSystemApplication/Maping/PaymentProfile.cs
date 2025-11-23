using AutoMapper;
using OrderManagementSystemApplication.Dtos.Payment;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<Payment, PaymentResponseDto>();

        }
    }
}
