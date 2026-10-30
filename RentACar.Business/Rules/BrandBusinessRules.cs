using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class BrandBusinessRules
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICarService _carService;

        public BrandBusinessRules(IBrandRepository brandRepository, ICarService carService)
        {
            _brandRepository = brandRepository;
            _carService = carService;
        }

        public async Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId)
        {
            return await _carService.CheckIfBrandIsUsedByAnyCarAsync(brandId);
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
    }
}
