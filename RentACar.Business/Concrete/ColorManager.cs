using AutoMapper;
using RentACar.Business.Abstract;
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

        public ColorManager(IRepository<Color> colorRepository, IMapper mapper)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(ColorAddDto colorAddDto)
        {
            var color = _mapper.Map<Color>(colorAddDto);
            await _colorRepository.AddAsync(color);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingColor = await _colorRepository.GetAsync(x => x.Id == id);
            if (existingColor == null)
            {
                return false;
            }
            await _colorRepository.DeleteAsync(existingColor);
            return true;
        }

        public async Task<List<ColorListDto>> GetAllAsync()
        {
            var colors = await _colorRepository.GetAllAsync();
            return _mapper.Map<List<ColorListDto>>(colors);
        }

        public async Task<ColorListDto?> GetByIdAsync(int id)
        {
            var color = await _colorRepository.GetAsync(x => x.Id == id);
            return _mapper.Map<ColorListDto>(color);
        }

        public async Task<bool> UpdateAsync(ColorUpdateDto colorUpdateDto)
        {
            var existingColor = await _colorRepository.GetAsync(x => x.Id == colorUpdateDto.Id);
            if (existingColor == null)
            {
                return false;
            }
            _mapper.Map(colorUpdateDto, existingColor);
            await _colorRepository.UpdateAsync(existingColor);
            return true;
        }
    }
}
