using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ColorDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminColorManager : IAdminColorService
    {
        private readonly IColorRepository _colorRepository;
        private readonly IMapper _mapper;
        private readonly ColorBusinessRules _colorBusinessRules;

        public AdminColorManager(IColorRepository colorRepository, IMapper mapper,ColorBusinessRules colorBusinessRules)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _colorBusinessRules = colorBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(ColorCreateDto colorAddDto)
        {
            colorAddDto.Name = colorAddDto.Name.Trim();
            IResult? result = BusinessRules.Run(
                await _colorBusinessRules.CheckIfColorNameExistsAsync(colorAddDto.Name)
                );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var color = _mapper.Map<Color>(colorAddDto);
            await _colorRepository.AddAsync(color);
            return new SuccessDataResult<int>(color.Id, "Renk başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingColor = await _colorRepository.GetAsync(x => x.Id == id);
            if (existingColor == null)
            {
                throw new BusinessException("Silinecek renk bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _colorBusinessRules.CheckIfColorIsUsedByAnyCarAsync(id)
                );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen hata oluştu!");
            }

            // SENİOR NOTU: Biz sadece silme emrini (DeleteAsync) veriyoruz. 
            // Fiziksel silme (Hard Delete) mi yoksa çöpe atma (Soft Delete) mı yapılacağına 
            // DataAccess katmanındaki DbContext (Gümrük Memuru) otonom olarak karar verir!
            await _colorRepository.DeleteAsync(existingColor);
            return new SuccessResult("Renk başarıyla silindi.");
        }

        public async Task<IResult> UpdateAsync(ColorUpdateDto colorUpdateDto)
        {
            colorUpdateDto.Name = colorUpdateDto.Name.Trim();

            IResult? result = BusinessRules.Run(
                await _colorBusinessRules.CheckIfColorNameExistsForUpdateAsync(colorUpdateDto.Name, colorUpdateDto.Id)
                );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
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

    }
}
