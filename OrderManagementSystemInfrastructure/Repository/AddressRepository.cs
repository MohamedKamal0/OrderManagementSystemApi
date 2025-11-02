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
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly DbSet<Address> _address;


        public AddressRepository(AppDbContext context) : base(context)
        {
            _address = context.Set<Address>();
        }
    
    }
}
