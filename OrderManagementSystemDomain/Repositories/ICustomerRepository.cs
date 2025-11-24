using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemDomain.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetCustomerWithOrdersAsync(int customerId);

    }
}
