using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.BrandDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly ICarService _carService;

        public BrandManager(IBrandRepository brandRepository, IMapper mapper, ICarService carService)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _carService = carService;
        }

        public async Task<IResult> AddAsync(BrandCreateDto brandAddDto)
        {
            // 1. TEMİZLİK: Gelen verinin sağındaki ve solundaki görünmez boşlukları tıraşla (Kandırmacayı önle)
            brandAddDto.Name = brandAddDto.Name.Trim();

            // 2. KONTROL: Asistanı (BusinessRules) çağır ve bekçinin raporunu okuması için ver.
            IResult? result = BusinessRules.Run(await CheckIfBrandNameExistAsync(brandAddDto.Name));
            if (result != null)
            {
                // BANT SORUMLUSU: Asistan hata raporuyla dönerse sistemi durdur ve kırmızı alarma bas! (Middleware yakalar)
                throw new BusinessException(result.Message ?? "Bu marka zaten sistemde kayıtlı! Lütfen başka marka deneyiniz.");
            }

            // 3. İŞLEM: Formu gerçek nesneye çevir ve veritabanına ekle.
            var brand = _mapper.Map<Brand>(brandAddDto);
            await _brandRepository.AddAsync(brand);

            return new SuccessResult("Marka başarıyla eklendi.");
        }


        public async Task<IResult> DeleteAsync(int id)
        {
            var existingBrand = await _brandRepository.GetAsync(x => x.Id == id);
            if (existingBrand == null)
            {
                throw new BusinessException("Silinecek marka bulunamadı.");
            }

            // SOFT DELETE (Yumuşak Silme): Veritabanından uçurmuyoruz, üzerini çiziyoruz.
            existingBrand.IsDeleted = true;
            existingBrand.DeletedDate = DateTime.UtcNow;

            // BAĞIMLILIK TEMİZLİĞİ: Marka silinirse, o markaya ait arabaları da vitrinden kaldır.
            var existingCars = await _carService.GetAllByBrandIdAsync(id);
            // Performasnlı değil büyük sistemlerde repository ye özel metot yazılır tek hamlede isDeleted true yapar
            foreach (var car in existingCars.Data)
            {
                await _carService.DeleteAsync(car.Id);
            }

            await _brandRepository.UpdateAsync(existingBrand);
            return new SuccessResult("Marka başarıyla silindi.");
        }

        public async Task<IDataResult<List<BrandResultDto>>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();
            var brandDtos = _mapper.Map<List<BrandResultDto>>(brands);
            return new SuccessDataResult<List<BrandResultDto>>(brandDtos, "Markalar başarıyla listelendi.");
        }

        public async Task<IDataResult<BrandResultDto>> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(x => x.Id == id);
            if (brand == null)
            {
                throw new BusinessException("Aranan marka bulunamadı.");
            }

            var brandDto = _mapper.Map<BrandResultDto>(brand);
            return new SuccessDataResult<BrandResultDto>(brandDto, "Marka başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto)
        {
            brandUpdateDto.Name = brandUpdateDto.Name.Trim();

            IResult? result = BusinessRules.Run(await CheckIfBrandNameExistsForUpdateAsync(brandUpdateDto.Name, brandUpdateDto.Id));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Bu marka zaten sistemde kayıtlı! Lütfen başka deneyiniz.");
            }

            var existingBrand = await _brandRepository.GetAsync(x => x.Id == brandUpdateDto.Id);
            if (existingBrand == null)
            {
                throw new BusinessException("Güncellenecek marka bulunamadı.");
            }

            _mapper.Map(brandUpdateDto, existingBrand);
            await _brandRepository.UpdateAsync(existingBrand);

            return new SuccessResult("Marka başarıyla güncellendi.");
        }

        // ILike ile büyük/küçük harf duyarsız (case-insensitive) esnek arama yapılır.
        // N-Tier Notu: Bu kullanım projeyi PostgreSQL'e bağımlı kılar, ancak orijinal büyük harf girişlerini bozmamak için bu mimari taviz (trade-off) verilmiştir.
        private async Task<IResult> CheckIfBrandNameExistAsync(string name)
        {
            bool existingBrand = await _brandRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, name));

            if (existingBrand)
            {
                return new ErrorResult("Bu marka zaten kayıtlı!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfBrandNameExistsForUpdateAsync(string name, int brandId)
        {
            bool isExist = await _brandRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, name) && x.Id != brandId);
            if (isExist)
            {
                return new ErrorResult("Bu marka zaten kayıtlı! Lütfen başka bir marka deneyiniz.");
            }
            return new SuccessResult();
        }
    }
}