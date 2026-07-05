using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RentACar.DataAccess.Abstract
{
    public interface IRepository<T>  where T : class
    {
        /// <summary>
        /// Veritabanındaki verileri listeler. İsteğe bağlı olarak filtre uygulanabilir.
        /// </summary>
        /// <param name="filter">Örn: x => x.DailyPrice > 500 gibi LINQ sorguları alır. Null ise tüm tabloyu çeker.</param>
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);


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
