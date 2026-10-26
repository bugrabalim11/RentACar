using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class BrandBusinessRules
    {
        private readonly ICarRepository _carRepository;

        public BrandBusinessRules(ICarRepository carRepository)
        {
            _carRepository = carRepository;
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
    }
}
