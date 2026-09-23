using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class ReferenceCheckService : IReferenceCheckService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICarRepository _carRepository;

        public ReferenceCheckService(IBrandRepository brandRepository, IColorRepository colorRepository, ICarRepository carRepository)
        {
            _brandRepository = brandRepository;
            _colorRepository = colorRepository;
            _carRepository = carRepository;
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

        public async Task<IResult> CheckIfColorExistsAsync(int colorId)
        {
            bool existingColor = await _colorRepository.AnyAsync(x => x.Id == colorId);
            if (existingColor)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu renk sistemde bulunamadı!");
        }

        public async Task<IResult> CheckIfColorIsUsedByAnyCarAsync(int colorId)
        {
            bool existingCar = await _carRepository.AnyAsync(x => x.ColorId == colorId);
            if (!existingCar)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu renk sistemdeki araçlar tarafından kullanıldığı için silinemez!");
        }
    }
}
