using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.OfficeDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class OfficeManager : IOfficeService
    {
        private readonly IOfficeRepository _officeRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<OfficeAddDto> _addValidator;
        private readonly IValidator<OfficeUpdateDto> _updateValidator;

        public OfficeManager(IOfficeRepository officeRepository, IMapper mapper, IValidator<OfficeAddDto> addValidator, IValidator<OfficeUpdateDto> updateValidator)
        {
            _officeRepository = officeRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(OfficeAddDto officeAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(officeAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
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
                return new ErrorResult("Silinecek ofis bulunamadı.");
            }

            await _officeRepository.DeleteAsync(existingOffice);
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
            var validationResult = await _updateValidator.ValidateAsync(officeUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
    }
}
