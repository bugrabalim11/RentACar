using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;
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
        private readonly IMapper _mapper;  // İşte bizim yetenekli aşçı yamağımız!

        public CarManager(IRepository<Car> carRepository, IMapper mapper)
        {
            _carRepository = carRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(CarAddDto carAddDto)
        {
            // 1. ADIM: Kullanıcının gönderdiği CarAddDto tabağını, SQL'in anladığı Car tenceresine çeviriyoruz
            var car = _mapper.Map<Car>(carAddDto);

            if (car.DailyPrice <= 0)
            {
                throw new ArgumentException("Günlük araç kiralama bedeli 0'dan büyük olmalıdır.");
            }

            // 2. ADIM: Artık elimizde temiz bir Car nesnesi var, Repository'ye paslayabiliriz
            await _carRepository.AddAsync(car);
        }

        public async Task DeleteAsync(Car car)
        {
            await _carRepository.DeleteAsync(car);
        }

        public async Task<List<CarListDto>> GetAllAsync()
        {
            // 1. ADIM: Veritabanından ham Car listesini (tencereleri) çekiyoruz
            var cars = await _carRepository.GetAllAsync();

            // 2. ADIM: AutoMapper'a diyoruz ki: "Şu ham arabalar listesini al, bana List<CarListDto> olarak geri ver"
            return _mapper.Map<List<CarListDto>>(cars);
        }

        public async Task<CarListDto?> GetByIdAsync(int id)
        {
            // Dün yazdığımız O sihirli Expression (x => x.Id == id) mantığı burada devreye giriyor!
            var car = await _carRepository.GetAsync(x => x.Id == id);

            // Bulduysa CarListDto'ya çevirir, bulamadıysa (null ise) güvenli bir şekilde null döner
            return _mapper.Map<CarListDto?>(car);
        }

        public async Task UpdateAsync(Car car)
        {
            await _carRepository.UpdateAsync(car);
        }
    }
}
