using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    // Genel depocu yeteneklerini base sınıftan alıyor, özel sözleşmeyi uyguluyor
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly RentACarContext _context;

        public CarRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsWithDetailsAsync()
        {
            // Entity Framework'ün Include kancalarını kullanarak tabloları SQL'de JOIN ediyoruz
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Color)
                .ToListAsync();
        }

        public async Task<Car?> GetCarWithDetailsAsync(int id)
        {
            // Arabalar tablosuna git, Marka ve Rengi dahil et, ID'si benim gönderdiğim ID olanı bul ve getir.
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Color)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
