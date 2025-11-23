using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemDomain.Repositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<Order?> GetOrderWithPaymentAsync(int orderId, int customerId);

    }
}
