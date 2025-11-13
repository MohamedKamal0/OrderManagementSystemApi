using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderManagementSystemApplication.Dtos.Customer;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class CustomerProfile:Profile
    {

        public CustomerProfile()
        {
             CreateMap<CustomerRegistrationDto, Customer>();
             CreateMap<Customer, CustomerResponseDto>();
             CreateMap<CustomerUpdateDto, Customer>();
        }
    }
}
