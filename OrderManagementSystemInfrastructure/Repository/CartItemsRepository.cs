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
    public class CartItemsRepository:GenericRepository<CartItem>, ICartItemsRepository
    {
        private readonly DbSet<CartItem> _cartItems;
        public CartItemsRepository(AppDbContext context) : base(context)
        {
            _cartItems = context.Set<CartItem>();
        }
    }
}
