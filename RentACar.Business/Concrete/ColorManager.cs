using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
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
        private readonly ICarService _carService;

        public ColorManager(IColorRepository colorRepository, IMapper mapper, ICarService carService)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _carService = carService;
        }

        public async Task<IResult> AddAsync(ColorCreateDto colorAddDto)
        {
            colorAddDto.Name = colorAddDto.Name.Trim();

            // ASİSTAN KONTROLÜ: Aynı renk isminden var mı?
            IResult? result = BusinessRules.Run(await CheckIfColorNameExistsAsync(colorAddDto.Name));
            if (result != null)
            {
                // Hata varsa kırmızı alarm!
                throw new BusinessException(result.Message ?? "Bu renk zaten sistemde kayıtlı! Lütfen başka deneyiniz.");
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
                throw new BusinessException("Silinecek renk bulunamadı.");
            }

            // KORUYUCU İŞ KURALI (Data Integrity): Bu rengi kullanan araçlar var mı?
            var existingCars = await _carService.GetCarsByColorIdAsync(id);
            if (existingCars.Data != null && existingCars.Data.Any())
            {
                // Rengi kullanan araba varsa silme işlemini şiddetle reddet!
                throw new BusinessException("Bu renk sistemdeki araçlar tarafından kullanıldığı için silinemez!");
            }

            // SOFT DELETE (Yumuşak Silme)
            existingColor.IsDeleted = true;
            existingColor.DeletedDate = DateTime.UtcNow;
            await _colorRepository.UpdateAsync(existingColor);
            return new SuccessResult("Renk başarıyla silindi.");
        }

        public async Task<IDataResult<List<ColorResultDto>>> GetAllAsync()
        {
            var colors = await _colorRepository.GetAllAsync();
            var colorDtos = _mapper.Map<List<ColorResultDto>>(colors);
            return new SuccessDataResult<List<ColorResultDto>>(colorDtos, "Renkler başarıyla listelendi.");
        }

        public async Task<IDataResult<ColorResultDto>> GetByIdAsync(int id)
        {
            var color = await _colorRepository.GetAsync(x => x.Id == id);
            if (color == null)
            {
                // BUM! Eski ErrorDataResult silindi, Kırmızı Alarm eklendi!
                throw new BusinessException("Aranan renk bulunamadı.");
            }

            var colorDto = _mapper.Map<ColorResultDto>(color);
            return new SuccessDataResult<ColorResultDto>(colorDto, "Renk başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(ColorUpdateDto colorUpdateDto)
        {
            colorUpdateDto.Name = colorUpdateDto.Name.Trim();

            IResult? result = BusinessRules.Run(await CheckIfColorNameExistsForUpdateAsync(colorUpdateDto.Name, colorUpdateDto.Id));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Bu renk zaten sistemde kayıtlı! Lütfen başka deneyiniz.");
            }

            var existingColor = await _colorRepository.GetAsync(x => x.Id == colorUpdateDto.Id);
            if (existingColor == null)
            {
                throw new BusinessException("Güncellenecek renk bulunamadı.");
            }

            _mapper.Map(colorUpdateDto, existingColor);
            await _colorRepository.UpdateAsync(existingColor);
            return new SuccessResult("Renk başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfColorNameExistsAsync(string colorName)
        {
            // ILike ile büyük/küçük harf duyarsız arama
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