using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminOfficeManager:IAdminOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;
        private readonly OfficeBusinessRules _officeBusinessRules;

        public AdminOfficeManager(IOfficeRepository officeRepository, IMapper mapper, OfficeBusinessRules officeBusinessRules)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _officeBusinessRules = officeBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(OfficeCreateDto officeAddDto)
        {
            officeAddDto.Name = officeAddDto.Name.Trim();
            officeAddDto.City = officeAddDto.City.Trim();
            officeAddDto.ContactNumber = officeAddDto.ContactNumber.Trim();
            IResult? result = BusinessRules.Run(
                await _officeBusinessRules.CheckIfOfficeExistsAsync(officeAddDto.Name)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var office = _mapper.Map<Office>(officeAddDto);
            await _officeRepository.AddAsync(office);
            return new SuccessDataResult<int>(office.Id, "Ofis başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingOffice = await _officeRepository.GetAsync(x => x.Id == id);
            if (existingOffice == null)
            {
                throw new BusinessException("Silinecek ofis bulunamadı.");
            }

            IResult? result = BusinessRules.Run(
                await _officeBusinessRules.CheckIfOfficeHasRentalsAsync(existingOffice.Id)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            await _officeRepository.DeleteAsync(existingOffice);
            return new SuccessResult("Ofis başarıyla silindi.");
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
            IResult? result = BusinessRules.Run(
                await _officeBusinessRules.CheckIfOfficeExistsForUpdateAsync(officeUpdateDto.Name, officeUpdateDto.Id)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(officeUpdateDto, existingOffice);
            await _officeRepository.UpdateAsync(existingOffice);
            return new SuccessResult("Ofis başarıyla güncellendi.");
        }
    }
}
