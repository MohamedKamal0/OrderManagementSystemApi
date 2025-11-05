using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemDomain.Repositories
{
    public interface IShoppingRepository:IGenericRepository<Cart>
    {
        Task<Cart?> GetActiveCartByCustomerAsync(int customerId);
        Task<Cart?> GetCartByIdAsync(int id);
    }
}
