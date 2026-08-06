using RentACar.Core.Entities;
using System.Linq.Expressions;

namespace RentACar.DataAccess.Abstract
{
    // KURAL: T tipi kesinlikle bir Sınıf (class) olmalı, IEntity VIP kartını taşımalı ve new'lenebilir olmalı!
    public interface IRepository<T> where T : class, IEntity, new()
    {
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null);

        /// <summary>
        /// Veritabanındaki verileri listeler. İsteğe bağlı olarak filtre uygulanabilir.
        /// </summary>
        /// <param name="filter">Örn: x => x.DailyPrice > 500 gibi LINQ sorguları alır. Null ise tüm tabloyu çeker.</param>
        /// <param name="ignoreQueryFilters">True gönderilirse Entity Framework'ün 'Görünmezlik Pelerini' (Global Query Filter) devre dışı kalır. Böylece silinmiş (IsDeleted=true) verileri de görebiliriz.</param>
        /// <returns></returns>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool ignoreQueryFilters = false);


        /// <summary>
        /// Filtreye uyan TEK BİR kayıt getirir. (Örn: Id'si 5 olan araba).
        /// Dikkat: Veri bulunamazsa 'null' dönebileceği için dönüş tipi T? (Nullable) yapılmıştır.
        /// </summary>
        Task<T?> GetAsync(Expression<Func<T, bool>> filter);

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
