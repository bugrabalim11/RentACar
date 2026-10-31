using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class CarBusinessRules
    {
        private readonly ICarRepository _carRepository;
        private readonly IBrandService _brandService;
        private readonly IColorService _colorService;

        public CarBusinessRules(ICarRepository carRepository, IBrandService brandService, IColorService colorService)
        {
            _carRepository = carRepository;
            _brandService = brandService;
            _colorService = colorService;
        }

        public async Task<IResult> CheckIfBrandExistsAsync(int brandId)
        {
            return await _brandService.CheckIfBrandExistsAsync(brandId);
        }

        public async Task<IResult> CheckIfColorExistsAsync(int colorId)
        {
            return await _colorService.CheckIfColorExistsAsync(colorId);
        }

        public async Task<IResult> CheckIfCarExistsAsync(int carId)
        {
            bool isExists = await _carRepository.AnyAsync(x => x.Id == carId);
            if (!isExists)
            {
                return new ErrorResult("Bu araç sistemde bulunamadı!");
            }
            return new SuccessResult();
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
    }
}
