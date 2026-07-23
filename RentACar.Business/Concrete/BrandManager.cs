using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Business.ValidationRules.BrandValidators;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.BrandDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<BrandAddDto> _addValidator;
        private readonly IValidator<BrandUpdateDto> _updateValidator;

        public BrandManager(IRepository<Brand> brandRepository, IMapper mapper, IValidator<BrandAddDto> addValidator, IValidator<BrandUpdateDto> updateValidator)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(BrandAddDto brandAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(brandAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var brand = _mapper.Map<Brand>(brandAddDto);
            await _brandRepository.AddAsync(brand);

            // ARTIK VOID (BOŞ) DÖNMÜYORUZ, KUTU DÖNÜYORUZ!
            return new SuccessResult("Marka başarıyla eklendi.");
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
            var validationResult = await _updateValidator.ValidateAsync(brandUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
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
    }
}
