using RentACar.Core.Entities;
using System.Linq.Expressions;

namespace RentACar.DataAccess.Abstract
{
    /// <summary>
    /// Veri erişim katmanı için temel (Generic) Repository sözleşmesidir.
    /// Tüm CRUD operasyonları ve LINQ tabanlı sorgulama işlemlerini standartlaştırır.
    /// </summary>
    public interface IRepository<T> where T : class, IEntity, new()
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

        /// <summary>
        /// Belirtilen filtreye (LINQ) uyan tüm kayıtları listeler. Filtre verilmezse tablonun tamamını döndürür.
        /// </summary>
        /// <param name="filter">Uygulanacak filtre ifadesi (Expression).</param>
        /// <param name="ignoreQueryFilters">True olarak ayarlanırsa, Entity Framework'ün Global Query Filter (Örn: Soft Delete) mekanizmasını devre dışı bırakır.</param>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false);


        /// <summary>
        /// Belirtilen filtreye uyan tek bir kaydı getirir.
        /// </summary>
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, bool ignoreQueryFilters = false);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
