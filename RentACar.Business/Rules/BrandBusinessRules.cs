using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class BrandBusinessRules
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICarReferenceService _carReferenceService;

        public BrandBusinessRules(IBrandRepository brandRepository, ICarReferenceService carReferenceService)
        {
            _brandRepository = brandRepository;
            _carReferenceService = carReferenceService;
        }

        public async Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId)
        {
            return await _carReferenceService.CheckIfBrandIsUsedByAnyCarAsync(brandId);
        }

        public async Task<IResult> CheckIfBrandNameExistAsync(string name)
        {
            bool existingBrand = await _brandRepository.AnyAsync(x => x.Name.ToLower() == name.ToLower());

            if (existingBrand)
            {
                return new ErrorResult("Bu marka zaten kayıtlı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfBrandNameExistsForUpdateAsync(string name, int brandId)
        {
            bool isExist = await _brandRepository.AnyAsync(x => x.Name.ToLower() == name.ToLower() && x.Id != brandId);
            if (isExist)
            {
                return new ErrorResult("Bu marka zaten kayıtlı! Lütfen başka bir marka deneyiniz.");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfBrandExistsAsync(int brandId)
        {
            bool existingBrand = await _brandRepository.AnyAsync(x => x.Id == brandId);
            if (existingBrand)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu marka sistemde bulunamadı!");
        }
    }
}
