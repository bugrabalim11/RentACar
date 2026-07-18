using Microsoft.EntityFrameworkCore;
using RentACar.Core.Entities;
using RentACar.DataAccess.Abstract;
using RentACar.DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace RentACar.DataAccess.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly RentACarContext _context;

        public Repository(RentACarContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            // Set<T>() o an hangi tabloyla çalışıyorsak (Örn: Cars) o tabloya konumlanır ve veriyi ekler
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            // Sadece şarta uyan kayıt var mı diye bakar, True/False döner.
            return await _context.Set<T>().AnyAsync(filter);
        }

        public async Task DeleteAsync(T entity)
        {
            // Silme işlemi önce RAM'de kaydın durumunu 'Silindi' olarak işaretler (O yüzden await yok)
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Çoklu kayıt (Liste) getirme işlemi
        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            // Ternary If (üçlü operatör) kullandık:
            // Eğer üst katmandan bir filtre gönderilmediyse (null ise) -> Tüm tabloyu listele (ToListAsync)
            // Eğer filtre gönderildiyse -> Önce Where(filter) ile filtrele, sonra listele (ToListAsync)
            return filter == null
                ? await _context.Set<T>().ToListAsync()
                : await _context.Set<T>().Where(filter).ToListAsync();
        }

        // Tek bir kayıt getirme işlemi
        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter)
        {
            // Set<T>() ilgili tabloya gider.
            // SingleOrDefaultAsync(filter) ise o tablodan gönderdiğimiz şarta uyan TEK BİR kaydı asenkron getirir.
            return await _context.Set<T>().SingleOrDefaultAsync(filter);
        }

        public async Task UpdateAsync(T entity)
        {
            // Güncelleme işlemi önce RAM'de (hafızada) kaydın durumunu 'Güncellendi' olarak işaretler (O yüzden await yok)
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
