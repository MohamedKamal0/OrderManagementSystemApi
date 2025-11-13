using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OrderManagementSystemApplication.Dtos.Category;
using OrderManagementSystemApplication.Dtos.Customer;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Maping
{
    public class CategoryProfile :Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryCreateDto, Category>();
            CreateMap<Category, CategoryResponseDto>();

        }

    }
}
