using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class OfficeManager : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;

        public OfficeManager(IOfficeRepository officeRepository, IMapper mapper)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(OfficeAddDto officeAddDto)
        {
            await CheckIfOfficeExistsAsync(officeAddDto.Name);

            var office = _mapper.Map<Office>(officeAddDto);
            await _officeRepository.AddAsync(office);
            return new SuccessResult("Ofis başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingOffice = await _officeRepository.GetAsync(x => x.Id == id);
            if (existingOffice == null)
            {
                return new ErrorResult("Silinecek ofis bulunamadı.");
            }

            existingOffice.Status = false;
            await _officeRepository.UpdateAsync(existingOffice);
            return new SuccessResult("Ofis başarıyla silindi.");
        }

        public async Task<IDataResult<List<OfficeListDto>>> GetAllAsync()
        {
            var offices = await _officeRepository.GetAllAsync();
            var officeDtos = _mapper.Map<List<OfficeListDto>>(offices);
            return new SuccessDataResult<List<OfficeListDto>>(officeDtos, "Ofisler başarıyla listelendi.");
        }

        public async Task<IDataResult<OfficeListDto>> GetByIdAsync(int id)
        {
            var office = await _officeRepository.GetAsync(x => x.Id == id);
            if (office == null)
            {
                return new ErrorDataResult<OfficeListDto>("Ofis bulunamadı.");
            }

            var officeDto = _mapper.Map<OfficeListDto>(office);
            return new SuccessDataResult<OfficeListDto>(officeDto, "Ofis başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(OfficeUpdateDto officeUpdateDto)
        {
            await CheckIfOfficeExistsForUpdateAsync(officeUpdateDto.Name,officeUpdateDto.Id);

            var existingOffice = await _officeRepository.GetAsync(x => x.Id == officeUpdateDto.Id);
            if (existingOffice == null)
            {
                return new ErrorResult("Güncellenecek ofis bulunamadı.");
            }

            // : Map(Kaynak, Hedef)
            _mapper.Map(officeUpdateDto, existingOffice);
            await _officeRepository.UpdateAsync(existingOffice);
            return new SuccessResult("Ofis başarıyla güncellendi.");
        }

        private async Task CheckIfOfficeExistsAsync(string officeName)
        {
            bool existingOffice = await _officeRepository.AnyAsync(x => x.Name == officeName);
            if (existingOffice)
            {
                throw new BusinessException("Bu ofis sistemde kayıtlı! Lütfen başka ofis girmeyi deneyin.");
            }
        }

        private async Task CheckIfOfficeExistsForUpdateAsync(string officeName, int officeId)
        {
            bool isExist = await _officeRepository.AnyAsync(x => x.Name == officeName && x.Id != officeId);
            if (isExist)
            {
                throw new BusinessException("Bu ofis sistemde kayıtlı! Lütfen başka ofis girmeyi deneyin.");
            }
        }
    }
}
