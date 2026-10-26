using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class CarImageBusinessRules
    {
        private readonly ICarImageRepository _carImageRepository;

        public CarImageBusinessRules(ICarImageRepository carImageRepository)
        {
            _carImageRepository = carImageRepository;
        }

        public async Task<IResult> CheckIfCarImageLimitExceededAsync(int carId)
        {
            var result = await _carImageRepository.CountAsync(x => x.CarId == carId);
            if (result >= 5)
            {
                return new ErrorResult("Bir arabanın en fazla 5 resmi olabilir.");
            }
            return new SuccessResult();
        }
    }
}
