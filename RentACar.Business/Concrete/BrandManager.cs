using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.BrandDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private readonly IRepository<Brand> _brandRepository;
        private readonly IMapper _mapper;

        public BrandManager(IRepository<Brand> brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(BrandAddDto brandAddDto)
        {
            var brand = _mapper.Map<Brand>(brandAddDto);

            await _brandRepository.AddAsync(brand);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(x => x.Id == id);
            if (brand == null)
            {
                return false;
            }

            await _brandRepository.DeleteAsync(brand);
            return true;
        }

        public async Task<List<BrandListDto>> GetAllAsync()
        {
            var brands = await _brandRepository.GetAllAsync();

            return _mapper.Map<List<BrandListDto>>(brands);
        }

        public async Task<BrandListDto?> GetByIdAsync(int id)
        {
            var brand = await _brandRepository.GetAsync(x => x.Id == id);

            return _mapper.Map<BrandListDto>(brand);
        }

        public async Task<bool> UpdateAsync(BrandUpdateDto brandUpdateDto)
        {
            var existingBrand = await _brandRepository.GetAsync(x => x.Id == brandUpdateDto.Id);
            if (existingBrand == null)
            {
                return false;
            }

            _mapper.Map(brandUpdateDto, existingBrand);
            return true;
        }
    }
}
