using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderManagementSystemApplication.Dtos.Address;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class AddressProfile:Profile
    {

        public AddressProfile()
        {
            // CreateMap<Source, Destination>();
            CreateMap<AddressCreateDto, Address>();
            CreateMap<AddressUpdateDto, Address>();
            CreateMap<Address, AddressResponseDto>();
        }
    }
}
