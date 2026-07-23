using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ContactInfoDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class ContactInfoManager : IContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ContactInfoAddDto> _addValidator;
        private readonly IValidator<ContactInfoUpdateDto> _updateValidator;

        public ContactInfoManager(IContactInfoRepository contactInfoRepository, IMapper mapper, IValidator<ContactInfoAddDto> addValidator, IValidator<ContactInfoUpdateDto> updateValidator)
        {
            _contactInfoRepository = contactInfoRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(ContactInfoAddDto contactInfoAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(contactInfoAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var contactInfo = _mapper.Map<ContactInfo>(contactInfoAddDto);
            await _contactInfoRepository.AddAsync(contactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingContactInfo = await _contactInfoRepository.GetAsync(x => x.Id == id);
            if (existingContactInfo == null)
            {
                return new ErrorResult("Silinecek iletişim bilgisi bulunamadı.");
            }

            existingContactInfo.Status = false;
            await _contactInfoRepository.UpdateAsync(existingContactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla silindi.");
        }

        public async Task<IDataResult<List<ContactInfoListDto>>> GetAllAsync()
        {
            var contactInfos = await _contactInfoRepository.GetAllAsync();
            var contactInfoDtos = _mapper.Map<List<ContactInfoListDto>>(contactInfos);
            return new SuccessDataResult<List<ContactInfoListDto>>(contactInfoDtos, "İletişim bilgileri başarıyla listelendi.");
        }

        public async Task<IDataResult<ContactInfoListDto>> GetByIdAsync(int id)
        {
            var contactInfo = await _contactInfoRepository.GetAsync(x => x.Id == id);
            if (contactInfo == null)
            {
                return new ErrorDataResult<ContactInfoListDto>("İletişim bilgisi bulunamadı.");
            }

            var contactInfoDto = _mapper.Map<ContactInfoListDto>(contactInfo);
            return new SuccessDataResult<ContactInfoListDto>(contactInfoDto, "İletişim bilgisi başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(ContactInfoUpdateDto contactInfoUpdateDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(contactInfoUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var existingContactInfo = await _contactInfoRepository.GetAsync(x => x.Id == contactInfoUpdateDto.Id);
            if (existingContactInfo == null)
            {
                return new ErrorResult("Güncellenecek iletişim bilgisi bulunamadı.");
            }

            // Bu satır harika çalışır, Tercüman (Mapper) DTO'daki yeni bilgileri alır ve veritabanından çektiğin existingContactInfo nesnesinin üzerine yazar.
            _mapper.Map(contactInfoUpdateDto, existingContactInfo);
            await _contactInfoRepository.UpdateAsync(existingContactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla güncellendi.");
        }
    }
}
