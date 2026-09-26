using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
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
        private readonly IReferenceCheckService _referenceCheckService;

        public OfficeManager(IOfficeRepository officeRepository, IMapper mapper, IReferenceCheckService referenceCheckService)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _referenceCheckService = referenceCheckService;
        }

        public async Task<IResult> AddAsync(OfficeCreateDto officeAddDto)
        {
            officeAddDto.Name = officeAddDto.Name.Trim();
            officeAddDto.City = officeAddDto.City.Trim();
            officeAddDto.ContactNumber = officeAddDto.ContactNumber.Trim();
            IResult? result = BusinessRules.Run(await CheckIfOfficeExistsAsync(officeAddDto.Name));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var office = _mapper.Map<Office>(officeAddDto);
            await _officeRepository.AddAsync(office);
            return new SuccessResult("Ofis başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingOffice = await _officeRepository.GetAsync(x => x.Id == id);
            if (existingOffice == null)
            {
                throw new BusinessException("Silinecek ofis bulunamadı.");
            }

            IResult? result = BusinessRules.Run(await _referenceCheckService.CheckIfOfficeHasRentalsAsync(existingOffice.Id));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            existingOffice.IsDeleted = true;
            existingOffice.DeletedDate = DateTime.UtcNow;
            await _officeRepository.UpdateAsync(existingOffice);
            return new SuccessResult("Ofis başarıyla silindi.");
        }

        public async Task<IDataResult<List<OfficeResultDto>>> GetAllAsync()
        {
            var offices = await _officeRepository.GetAllAsync();
            var officeDtos = _mapper.Map<List<OfficeResultDto>>(offices);
            return new SuccessDataResult<List<OfficeResultDto>>(officeDtos, "Ofisler başarıyla listelendi.");
        }

        public async Task<IDataResult<OfficeResultDto>> GetByIdAsync(int id)
        {
            var office = await _officeRepository.GetAsync(x => x.Id == id);
            if (office == null)
            {
                throw new BusinessException("Ofis bulunamadı.");
            }

            var officeDto = _mapper.Map<OfficeResultDto>(office);
            return new SuccessDataResult<OfficeResultDto>(officeDto, "Ofis başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(OfficeUpdateDto officeUpdateDto)
        {
            var existingOffice = await _officeRepository.GetAsync(x => x.Id == officeUpdateDto.Id);
            if (existingOffice == null)
            {
                throw new BusinessException("Güncellenecek ofis bulunamadı.");
            }

            officeUpdateDto.Name = officeUpdateDto.Name.Trim();
            officeUpdateDto.City = officeUpdateDto.City.Trim();
            officeUpdateDto.ContactNumber = officeUpdateDto.ContactNumber.Trim();
            IResult? result = BusinessRules.Run(await CheckIfOfficeExistsForUpdateAsync(officeUpdateDto.Name, officeUpdateDto.Id));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // : Map(Kaynak, Hedef)
            _mapper.Map(officeUpdateDto, existingOffice);
            await _officeRepository.UpdateAsync(existingOffice);
            return new SuccessResult("Ofis başarıyla güncellendi.");
        }

        private async Task<IResult> CheckIfOfficeExistsAsync(string officeName)
        {
            bool existingOffice = await _officeRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, officeName));
            if (existingOffice)
            {
                return new ErrorResult("Bu ofis sistemde kayıtlı! Lütfen başka ofis girmeyi deneyin.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfOfficeExistsForUpdateAsync(string officeName, int officeId)
        {
            bool isExist = await _officeRepository.AnyAsync(x => Microsoft.EntityFrameworkCore.EF.Functions.ILike(x.Name, officeName) && x.Id != officeId);
            if (isExist)
            {
                return new ErrorResult("Bu ofis sistemde kayıtlı! Lütfen başka ofis girmeyi deneyin.");
            }
            return new SuccessResult();
        }
    }
}
