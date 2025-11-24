using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemDomain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<Address> Addresses { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IGenericRepository<Payment> Payments { get; }
        IGenericRepository<Cancellation> Cancellations { get; }
        IGenericRepository<Category> Categorys { get; }
        IGenericRepository<Product> Products { get; }
        IShoppingRepository Carts { get; }




        int Complete();

    }
}
