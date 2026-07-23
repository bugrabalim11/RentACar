using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ColorDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class ColorManager : IColorService
    {
        private readonly IRepository<Color> _colorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ColorAddDto> _addValidator;
        private readonly IValidator<ColorUpdateDto> _updateValidator;

        public ColorManager(IRepository<Color> colorRepository, IMapper mapper, IValidator<ColorAddDto> addValidator, IValidator<ColorUpdateDto> updateValidator)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(ColorAddDto colorAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(colorAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var color = _mapper.Map<Color>(colorAddDto);
            await _colorRepository.AddAsync(color);
            return new SuccessResult("Renk başarıyla eklendi.");
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
            var validationResult = await _updateValidator.ValidateAsync(colorUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
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
    }
}
