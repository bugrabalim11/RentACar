using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class CarReferenceManager : ICarReferenceService
    {
        private readonly ICarRepository _carRepository;

        public CarReferenceManager(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IResult> CheckIfBrandIsUsedByAnyCarAsync(int brandId)
        {
            bool isExists = await _carRepository.AnyAsync(x => x.BrandId == brandId);
            if (isExists)
            {
                return new ErrorResult("Bu markaya ait bir araç sistemde kaytılı olduğu için silinemez!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfColorIsUsedByAnyCarAsync(int colorId)
        {
            bool existingCar = await _carRepository.AnyAsync(x => x.ColorId == colorId);
            if (existingCar)
            {
                return new ErrorResult("Bu renk sistemdeki araçlar tarafından kullanıldığı için silinemez!");
            }
            return new SuccessResult();
        }
    }
}
