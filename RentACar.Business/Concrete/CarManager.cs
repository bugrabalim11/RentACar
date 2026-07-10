using AutoMapper;
using FluentValidation;
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

        // 1. Güvenlik görevlimizi (Validator) tanımlıyoruz. 
        // Sadece CarAddDto'dan anlayan bir güvenlik görevlisi istiyoruz.
        private readonly IValidator<CarAddDto> _validator;

        // 2. Constructor'a (Yapıcı Metot) ekleyerek sisteme "Bana bu görevliyi getir" diyoruz.
        public CarManager(IRepository<Car> carRepository, IMapper mapper, IValidator<CarAddDto> validator)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task AddAsync(CarAddDto carAddDto)
        {
            // 3. Mutfağa veri girdiği an ilk iş güvenlik görevlisine kontrol ettiriyoruz.
            var validationResult = await _validator.ValidateAsync(carAddDto);

            // 4. Eğer kural ihlali varsa (Örn: Fiyat eksi girildiyse)
            if (!validationResult.IsValid)
            {
                // Sistemin sigortasını attırıyoruz! (Exception fırlatıyoruz)
                // Hataları da içine koyuyoruz ki ileride kullanıcıya "Şuraları yanlış girdin" diyebilelim.
                throw new ValidationException(validationResult.Errors);
            }

            // Eğer kod buraya ulaştıysa, Validator "Geçebilir" demiştir. Yemeği pişiriyoruz.
            var car = _mapper.Map<Car>(carAddDto);  // DTO'yu Entity'e çeviriyoruz
            await _carRepository.AddAsync(car);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var car = await _carRepository.GetAsync(x => x.Id == id);

            if (car == null)
            {
                return false;  // Araba yok, işlemi iptal et ve Garsona 'false' dön!
            }

            await _carRepository.DeleteAsync(car);
            return true;  // Başarıyla silindi
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

        public async Task<bool> UpdateAsync(CarUpdateDto carUpdateDto)
        {
            var existingCar = await _carRepository.GetAsync(x => x.Id == carUpdateDto.Id);

            if (existingCar == null)
            {
                return false;  // Araba yok!
            }

            // AutoMapper ile yeni gelen DTO'daki bilgileri, veritabanından bulduğumuz mevcut arabanın üstüne yazıyoruz
            _mapper.Map(carUpdateDto, existingCar);

            await _carRepository.UpdateAsync(existingCar);
            return true;
        }
    }
}
