using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class CarBusinessRules
    {
        private readonly ICarRepository _carRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IColorRepository _colorRepository;

        public CarBusinessRules(ICarRepository carRepository, IBrandRepository brandRepository, IColorRepository colorRepository)
        {
            _carRepository = carRepository;
            _brandRepository = brandRepository;
            _colorRepository = colorRepository;
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

        public async Task<IResult> CheckIfCarPlateExistsAsync(string plate)
        {

            bool existingPlate = await _carRepository.AnyAsync(x => x.Plate == plate);
            if (existingPlate)
            {
                return new ErrorResult("Bu plaka zaten sistemde kayıtlı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfCarPlateExitsForUpdateAsync(string plate, int currentCarId)
        {

            bool isExist = await _carRepository.AnyAsync(x => x.Plate == plate && x.Id != currentCarId);

            if (isExist)
            {
                return new ErrorResult("Bu plaka zaten sistemde kayıtlı.");
            }
            return new SuccessResult();
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
