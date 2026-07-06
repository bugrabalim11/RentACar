using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class CarManager : ICarService
    {
        // İşte dün yazdığımız o kusursuz Generic Repository! T yerine Car verdik.
        private readonly IRepository<Car> _carRepository;

        public CarManager(IRepository<Car> carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task AddAsync(Car car)
        {
            if (car.DailyPrice <= 0)
            {
                throw new ArgumentException("Günlük araç kiralama bedeli 0'dan büyük olmalıdır.");
            }

            await _carRepository.AddAsync(car);
        }

        public async Task DeleteAsync(Car car)
        {
            await _carRepository.DeleteAsync(car);
        }

        public async Task<List<Car>> GetAllAsync()
        {
            // İstersek buraya filtre de gönderebilirdik, ama şimdilik hepsini istiyoruz.
            return await _carRepository.GetAllAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            // Dün yazdığımız O sihirli Expression (x => x.Id == id) mantığı burada devreye giriyor!
            return await _carRepository.GetAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Car car)
        {
            await _carRepository.UpdateAsync(car);
        }
    }
}
