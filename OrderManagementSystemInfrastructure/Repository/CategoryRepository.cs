using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;
using OrderManagementSystemInfrastructure.Data;

namespace OrderManagementSystemInfrastructure.Repository
{
    public class CategoryRepository:GenericRepository<Category>, ICategoryRepository
    {
        private readonly DbSet<Category> _categories;
        public CategoryRepository(AppDbContext context) : base(context)
        {
            _categories = context.Set<Category>();
        }
    }
}
