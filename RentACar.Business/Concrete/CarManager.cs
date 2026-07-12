using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RentACar.Business.Concrete
{
    public class CarManager : ICarService
    {
        // ESKİSİ: private readonly IRepository<Car> _carRepository;
        private readonly ICarRepository _carRepository;
        private readonly IMapper _mapper;  // İşte bizim yetenekli aşçı yamağımız!

        // 1. Güvenlik görevlimizi (Validator) tanımlıyoruz. 
        // Sadece CarAddDto'dan anlayan bir güvenlik görevlisi istiyoruz.
        private readonly IValidator<CarAddDto> _addValidator;
        private readonly IValidator<CarUpdateDto> _updateValidator;

        // 2. Constructor'a (Yapıcı Metot) ekleyerek sisteme "Bana bu görevliyi getir" diyoruz.
        public CarManager(ICarRepository carRepository, IMapper mapper, IValidator<CarAddDto> addValidator, IValidator<CarUpdateDto> updateValidator)
        {
            _carRepository = carRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(CarAddDto carAddDto)
        {
            // 3. Mutfağa veri girdiği an ilk iş güvenlik görevlisine kontrol ettiriyoruz.
            var validationResult = await _addValidator.ValidateAsync(carAddDto);

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
            return new SuccessResult("Araba başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var car = await _carRepository.GetAsync(x => x.Id == id);
            if (car == null)
            {
                return new ErrorResult("Silincek araç bulunamadı.");
            }

            await _carRepository.DeleteAsync(car);
            return new SuccessResult("Araç başarıyla silindi.");
        }

        public async Task<IDataResult<List<CarListDto>>> GetAllAsync()
        {
            // İşte senin DataAccess'te yazdığın o özel Join'li metodu çağırıyoruz!
            var cars = await _carRepository.GetCarsWithDetailsAsync();

            // Arabalar, markaları ve renkleriyle beraber geldi. Şimdi onları şık tabaklara (DTO) koyalım.
            var carListDtos = _mapper.Map<List<CarListDto>>(cars);

            // Kargo kutusuna koy ve yolla!
            return new SuccessDataResult<List<CarListDto>>(carListDtos, "Arabalar başarıyla listelendi.");
        }


        public async Task<IDataResult<CarListDto>> GetByIdAsync(int id)
        {
            // ESKİSİ: var car = await _carRepository.GetAsync(x => x.Id == id);
            // YENİSİ: Artık Join'li veriyi getiren kendi özel metodumuzu kullanıyoruz!
            var car = await _carRepository.GetCarWithDetailsAsync(id);
            if (car == null)
            {
                return new ErrorDataResult<CarListDto>("Aranan araç bulunamadı.");
            }

            // Bulduysa CarListDto'ya çevirir, bulamadıysa (null ise) güvenli bir şekilde null döner
            var carListDto = _mapper.Map<CarListDto>(car);
            return new SuccessDataResult<CarListDto>(carListDto, "Araba detayı getirildi.");
        }

        public async Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto)
        {
            // 1. KAPI KONTROLÜ (Validation - Çöp veri varsa veritabanını hiç yorma!)
            var validationResult = await _updateValidator.ValidateAsync(carUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 2. VERİTABANI KONTROLÜ (Senin sağlam mantığın)
            var existingCar = await _carRepository.GetAsync(x => x.Id == carUpdateDto.Id);
            if (existingCar == null)
            {
                return new ErrorResult("Güncellencek araç bulunamadı.");
            }

            // 3. EŞLEŞTİRME VE KAYIT
            // AutoMapper ile yeni gelen verileri, bulduğumuz mevcut arabanın üstüne yazıyoruz
            _mapper.Map(carUpdateDto, existingCar);
            await _carRepository.UpdateAsync(existingCar);
            return new SuccessResult("Araç başarıyla güncellendi.");
        }
    }
}
