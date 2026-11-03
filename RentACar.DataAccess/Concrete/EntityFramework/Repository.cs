using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities;
using RentACar.DataAccess.Abstract;
using System.Linq.Expressions;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    /// <summary>
    /// Entity Framework Core kullanarak IRepository sözleşmesini uygulayan temel (Generic) veri erişim sınıfıdır.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly RentACarContext _context;

        public Repository(RentACarContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query= _context.Set<T>();
            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }
            return await query.AnyAsync(filter);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
        {
            return filter == null ? await _context.Set<T>().CountAsync() : await _context.Set<T>().CountAsync(filter);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return filter == null
                ? await query.ToListAsync()
                : await query.Where(filter).ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (ignoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            return await query.SingleOrDefaultAsync(filter);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
