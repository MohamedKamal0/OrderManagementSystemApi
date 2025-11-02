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
    public class ProductRepository :GenericRepository<Product>, IProductRepository
    {
        private readonly DbSet<Product> _products;
        public ProductRepository(AppDbContext context) : base(context)
        {
            _products = context.Set<Product>();
        }
    }
}
