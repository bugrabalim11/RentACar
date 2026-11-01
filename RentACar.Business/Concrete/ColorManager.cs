using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ColorDtos;

namespace RentACar.Business.Concrete
{
    public class ColorManager : IColorService
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly ColorBusinessRules _colorBusinessRules;

        public ColorManager(IColorRepository colorRepository, IMapper mapper, ColorBusinessRules colorBusinessRules)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _colorBusinessRules = colorBusinessRules;
        }

        public async Task<IResult> CheckIfColorExistsAsync(int colorId)
        {
            return await _colorBusinessRules.CheckIfColorExistsAsync(colorId);
        }

        public async Task<IDataResult<List<ColorResultDto>>> GetAllAsync()
        {
            var colors = await _colorRepository.GetAllAsync();
            var sortedColors = colors.OrderByDescending(x => x.CreatedDate).ToList();
            var colorDtos = _mapper.Map<List<ColorResultDto>>(sortedColors);
            return new SuccessDataResult<List<ColorResultDto>>(colorDtos, "Renkler başarıyla listelendi.");
        }

        public async Task<IDataResult<ColorResultDto>> GetByIdAsync(int id)
        {
            var color = await _colorRepository.GetAsync(x => x.Id == id);
            if (color == null)
            {
                throw new BusinessException("Aranan renk bulunamadı.");
            }
            var colorDto = _mapper.Map<ColorResultDto>(color);
            return new SuccessDataResult<ColorResultDto>(colorDto, "Renk başarıyla getirildi.");
        }
    }
}