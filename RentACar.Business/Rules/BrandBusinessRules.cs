using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class BrandBusinessRules
    {
        private readonly ICarRepository _carRepository;
        private readonly IBrandRepository _brandRepository;

        public BrandBusinessRules(ICarRepository carRepository, IBrandRepository brandRepository)
        {
            _carRepository = carRepository;
            _brandRepository = brandRepository;
        }

        public async Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId)
        {
            bool existingCar = await _carRepository.AnyAsync(c => c.BrandId == brandId);
            if (existingCar)
            {
                return new ErrorResult("Bu markaya ait sistemde araçlar bulunduğu için marka silinemez!");
            }
            return new SuccessResult();
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
