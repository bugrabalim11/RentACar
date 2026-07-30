using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ColorDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class ColorManager : IColorService
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;

        public ColorManager(IColorRepository colorRepository, IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(ColorAddDto colorAddDto)
        {
            colorAddDto.Name = colorAddDto.Name.Trim();
            IResult? result = BusinessRules.Run(await CheckIfColorNameExistsAsync(colorAddDto.Name));
            if (result != null)
            {
                return result;
            }

            var color = _mapper.Map<Color>(colorAddDto);
            await _colorRepository.AddAsync(color);
            return new SuccessResult("Renk başarıyla eklendi.");
        }


        // Bu metot bu dükkanın kendi içindeki Update / Delete işlemleri için DEĞİL, dışarıdan(CarManager gibi) gelen
        // 'Renk var mı?' sorgularına yanıt vermek için açık bırakılmıştır.Ölü kod (Dead Code) değildir.
        public async Task<IResult> CheckIfColorExistsAsync(int id)
        {
            bool existingColor = await _colorRepository.AnyAsync(x => x.Id == id);
            if (existingColor)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu renk sistemde bulunamadı!");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingColor = await _colorRepository.GetAsync(x => x.Id == id);
            if (existingColor == null)
            {
                return new ErrorResult("Silinecek renk bulunamadı.");
            }

            existingColor.Status = false;
            await _colorRepository.UpdateAsync(existingColor);
            return new SuccessResult("Renk başarıyla silindi.");
        }

        public async Task<IDataResult<List<ColorListDto>>> GetAllAsync()
        {
            var colors = await _colorRepository.GetAllAsync();
            var colorDtos = _mapper.Map<List<ColorListDto>>(colors);
            return new SuccessDataResult<List<ColorListDto>>(colorDtos, "Renkler başarıyla listelendi.");
        }

        public async Task<IDataResult<ColorListDto>> GetByIdAsync(int id)
        {
            var color = await _colorRepository.GetAsync(x => x.Id == id);
            if (color == null)
            {
                return new ErrorDataResult<ColorListDto>("Aranan renk bulunamadı.");
            }

            var colorDto = _mapper.Map<ColorListDto>(color);
            return new SuccessDataResult<ColorListDto>(colorDto, "Renk başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(ColorUpdateDto colorUpdateDto)
        {
            colorUpdateDto.Name = colorUpdateDto.Name.Trim();

            IResult? result = BusinessRules.Run(await CheckIfColorNameExistsForUpdateAsync(colorUpdateDto.Name, colorUpdateDto.Id));
            if (result != null)
            {
                return result;
            }

            var existingColor = await _colorRepository.GetAsync(x => x.Id == colorUpdateDto.Id);
            if (existingColor == null)
            {
                return new ErrorResult("Güncellenecek renk bulunamadı.");
            }

            _mapper.Map(colorUpdateDto, existingColor);
            await _colorRepository.UpdateAsync(existingColor);
            return new SuccessResult("Renk başarıyla güncellendi.");
        }


        private async Task<IResult> CheckIfColorNameExistsAsync(string colorName)
        {
            bool existColorName = await _colorRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, colorName));
            if (existColorName)
            {
                return new ErrorResult("Bu renk zaten kayıtlı!");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfColorNameExistsForUpdateAsync(string colorName, int colorId)
        {
            bool isExist = await _colorRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, colorName) && x.Id != colorId);
            if (isExist)
            {
                return new ErrorResult("Bu renk zaten sistemde kayıtlı!");
            }
            return new SuccessResult();
        }
    }
}
