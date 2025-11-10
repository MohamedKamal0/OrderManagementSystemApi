using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;
using OrderManagementSystemInfrastructure.Data;
using Org.BouncyCastle.Asn1.X509;

namespace OrderManagementSystemInfrastructure.Repository
{
    public class OrderRepository:GenericRepository<Order>,IOrderRepository
    {

        private readonly DbSet<Order> _order;
        public OrderRepository(AppDbContext context) : base(context)
        {
            _order = context.Set<Order>();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _order
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Customer)
                .Include(o => o.BillingAddress)
                .Include(o => o.ShippingAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
