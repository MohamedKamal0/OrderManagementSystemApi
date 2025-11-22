using Microsoft.EntityFrameworkCore;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;
using OrderManagementSystemInfrastructure.Data;

namespace OrderManagementSystemInfrastructure.Repository
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly DbSet<Payment> _payment;
        public PaymentRepository(AppDbContext context) : base(context)
        {
            _payment = context.Set<Payment>();

        }
    }
}
