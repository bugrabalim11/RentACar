using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class ColorBusinessRules
    {
        private readonly IColorRepository _colorRepository;
        private readonly ICarRepository _carRepository;

        public ColorBusinessRules(IColorRepository colorRepository, ICarRepository carRepository)
        {
            _colorRepository = colorRepository;
            _carRepository = carRepository;
        }

        public async Task<IResult> CheckIfColorNameExistsAsync(string colorName)
        {
            bool existColorName = await _colorRepository.AnyAsync(x => x.Name.ToLower() == colorName.ToLower());
            if (existColorName)
            {
                return new ErrorResult("Bu renk zaten kayıtlı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfColorNameExistsForUpdateAsync(string colorName, int colorId)
        {
            bool isExist = await _colorRepository.AnyAsync(x => x.Name.ToLower() == colorName.ToLower() && x.Id != colorId);
            if (isExist)
            {
                return new ErrorResult("Bu renk zaten sistemde kayıtlı!");
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

        public async Task<IResult> CheckIfColorExistsAsync(int colorId)
        {
            bool existingColor = await _colorRepository.AnyAsync(x => x.Id == colorId);
            if (existingColor)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu renk sistemde bulunamadı!");
        }
    }
}
