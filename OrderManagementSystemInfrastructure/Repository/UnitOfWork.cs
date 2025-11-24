using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;
using OrderManagementSystemInfrastructure.Data;

namespace OrderManagementSystemInfrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {


        private readonly AppDbContext _context;


        public IGenericRepository<Address> Addresses { get; private set; }

        public ICustomerRepository Customers { get; private set; }

        public IGenericRepository<Payment> Payments { get; private set; }

        public IGenericRepository<Cancellation> Cancellations { get; private set; }

        public IGenericRepository<Category> Categorys { get; private set; }

        public IGenericRepository<Product> Products { get; private set; }


        public IShoppingRepository Carts { get; private set; }

        public IOrderRepository Orders { get; private set; }


        public UnitOfWork(AppDbContext context)
        {
            _context = context;


            Addresses = new GenericRepository<Address>(_context);
            Customers = new CustomerRepository(_context);
            Orders = new OrderRepository(_context);
            Payments = new GenericRepository<Payment>(_context);
            Cancellations = new GenericRepository<Cancellation>(_context);
            Categorys = new GenericRepository<Category>(_context);
            Products = new GenericRepository<Product>(_context);
            Carts = new ShoppingRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
