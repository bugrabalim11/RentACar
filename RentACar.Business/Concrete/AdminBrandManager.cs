using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.BrandDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminBrandManager : IAdminBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly BrandBusinessRules _brandBusinessRules;

        public AdminBrandManager(IBrandRepository brandRepository, IMapper mapper,BrandBusinessRules brandBusinessRules)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _brandBusinessRules = brandBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(BrandCreateDto brandAddDto)
        {
            brandAddDto.Name = brandAddDto.Name.Trim();
            IResult? result = BusinessRules.Run(
                await _brandBusinessRules.CheckIfBrandNameExistAsync(brandAddDto.Name));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Bu marka zaten sistemde kayıtlı! Lütfen başka marka deneyiniz.");
            }

            var brand = _mapper.Map<Brand>(brandAddDto);
            await _brandRepository.AddAsync(brand);
            return new SuccessDataResult<int>(brand.Id, "Marka başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingBrand = await _brandRepository.GetAsync(x => x.Id == id);
            if (existingBrand == null)
            {
                throw new BusinessException("Silinecek marka bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _brandBusinessRules.CheckIfBrandIsUsedByAnyCarAsync(id)
                );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // SENİOR NOTU: Biz sadece silme emrini (DeleteAsync) veriyoruz. 
            // Fiziksel silme (Hard Delete) mi yoksa çöpe atma (Soft Delete) mı yapılacağına 
            // DataAccess katmanındaki DbContext (Gümrük Memuru) otonom olarak karar verir!
            await _brandRepository.DeleteAsync(existingBrand);
            return new SuccessResult("Marka başarıyla silindi.");
        }

        public async Task<IResult> UpdateAsync(BrandUpdateDto brandUpdateDto)
        {
            brandUpdateDto.Name = brandUpdateDto.Name.Trim();

            IResult? result = BusinessRules.Run(
                await _brandBusinessRules.CheckIfBrandNameExistsForUpdateAsync(brandUpdateDto.Name, brandUpdateDto.Id));
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
    }
}
