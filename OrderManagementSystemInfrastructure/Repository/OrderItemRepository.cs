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
   public class OrderItemRepository :GenericRepository<OrderItem>,IOrderItemRepository
    {
        private readonly DbSet<OrderItem> _orders;
        public OrderItemRepository(AppDbContext context) : base(context)
        {
            _orders = context.Set<OrderItem>();
        }
    }
}
