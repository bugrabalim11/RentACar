using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class ColorBusinessRules
    {
        private readonly IColorRepository _colorRepository;
        private readonly ICarReferenceService _carReferenceService;

        public ColorBusinessRules(IColorRepository colorRepository, ICarReferenceService carReferenceService)
        {
            _colorRepository = colorRepository;
            _carReferenceService = carReferenceService;
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
            return await _carReferenceService.CheckIfColorIsUsedByAnyCarAsync(colorId);
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
