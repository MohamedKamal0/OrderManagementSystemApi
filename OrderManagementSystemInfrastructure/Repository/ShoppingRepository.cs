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
    public class ShoppingRepository:GenericRepository<Cart>, IShoppingRepository
    {
        private readonly DbSet<Cart> _shopping;
        public ShoppingRepository(AppDbContext context) : base(context)
        {
            _shopping = context.Set<Cart>();
        }

        public async Task<Cart?> GetActiveCartByCustomerAsync(int customerId)
        {
            return await _shopping
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);
        }

        public async Task<Cart?> GetCartByIdAsync(int id)
        {
            return await _shopping
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
