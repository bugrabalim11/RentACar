using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
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

        public BrandManager(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(BrandAddDto brandAddDto)
        {
            // Gelen verinin sağındaki ve solundaki görünmez boşlukları tıraşla (Trim) ve küçük harfe çevir.
            brandAddDto.Name = brandAddDto.Name.Trim();

            // Asistanı çağır ve bekçinin raporunu kucağına (params) ver:
            IResult? result = BusinessRules.Run(await CheckIfBrandNameExistAsync(brandAddDto.Name));

            // Asistanın getirdiği sonuca bak:
            if (result != null)
            {
                // Eğer result null değilse, asistan bir ceza makbuzu (ErrorResult) bulmuş demektir.
                // Hiç veritabanı işlemlerine girmeden direkt bu hatayı vezneye yolla!
                return result;
            }

            var brand = _mapper.Map<Brand>(brandAddDto);
            await _brandRepository.AddAsync(brand);

            // ARTIK VOID (BOŞ) DÖNMÜYORUZ, KUTU DÖNÜYORUZ!
            return new SuccessResult("Marka başarıyla eklendi.");
        }


        // Bu metot bu dükkanın kendi içindeki Update / Delete işlemleri için DEĞİL, dışarıdan(CarManager gibi) gelen
        // 'Marka var mı?' sorgularına yanıt vermek için açık bırakılmıştır.Ölü kod (Dead Code) değildir.
        public async Task<IResult> CheckIfBrandExistsAsync(int id)
        {
            bool existingBrand = await _brandRepository.AnyAsync(x => x.Id == id);
            if (existingBrand)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Aranan marka sistemde bulunamadı.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingBrand = await _brandRepository.GetAsync(x => x.Id == id);
            if (existingBrand == null)
            {
                return new ErrorResult("Silinecek marka bulunamadı.");
            }

            existingBrand.Status = false;
            await _brandRepository.UpdateAsync(existingBrand);
            return new SuccessResult("Marka başarıyla silindi.");
        }

        public async Task<IDataResult<List<BrandListDto>>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();

            var brandDtos = _mapper.Map<List<BrandListDto>>(brands);

            return new SuccessDataResult<List<BrandListDto>>(brandDtos, "Markalar başarıyla listelendi.");
        }

        public async Task<IDataResult<BrandListDto>> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(x => x.Id == id);
            if (brand == null)
            {
                return new ErrorDataResult<BrandListDto>("Aranan marka bulunamadı.");
            }

            var brandDto = _mapper.Map<BrandListDto>(brand);
            return new SuccessDataResult<BrandListDto>(brandDto, "Marka başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto)
        {
            brandUpdateDto.Name = brandUpdateDto.Name.Trim();
            IResult? result = BusinessRules.Run(await CheckIfBrandNameExistsForUpdateAsync(brandUpdateDto.Name, brandUpdateDto.Id));
            if (result != null)
            {
                return result;
            }

            var existingBrand = await _brandRepository.GetAsync(x => x.Id == brandUpdateDto.Id);
            if (existingBrand == null)
            {
                // ARTIK FALSE YERİNE ERROR RESULT KUTUSU DÖNÜYORUZ!
                return new ErrorResult("Güncellenecek marka bulunamadı.");
            }

            _mapper.Map(brandUpdateDto, existingBrand);
            await _brandRepository.UpdateAsync(existingBrand);

            // ARTIK TRUE YERİNE SUCCESS RESULT KUTUSU DÖNÜYORUZ!
            return new SuccessResult("Marka başarıyla güncellendi.");
        }


        // ILike ile büyük/küçük harf duyarsız (case-insensitive) esnek arama yapılır.
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
