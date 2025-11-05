using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace OrderManagementSystemDomain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();//no
        void Commit();//no
        void RollBack();//no
        IQueryable<T> GetTableNoTracking();//no
        IQueryable<T> GetTableAsTracking();//no
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(ICollection<T> entities);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

    }
}
