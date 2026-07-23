using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.DataAccess.Concrete;
using RentACar.DataAccess.Concrete.EntityFramework;
using RentACar.Dtos.ContactMessageDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class ContactMessageManager : IContactMessageService
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<ContactMessageAddDto> _addValidator;

        public ContactMessageManager(IContactMessageRepository contactMessageRepository, IMapper mapper, IValidator<ContactMessageAddDto> addValidator)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
            _addValidator = addValidator;
        }

        public async Task<IResult> AddAsync(ContactMessageAddDto contactMessageAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(contactMessageAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var contactMessage = _mapper.Map<ContactMessage>(contactMessageAddDto);

            contactMessage.SendDate = DateTime.UtcNow;
            contactMessage.IsRead = false;

            await _contactMessageRepository.AddAsync(contactMessage);
            return new SuccessResult("Mesajınız başarıyla gönderildi.");
        }

        public async Task<IResult> ChangeIsReadStatusAsync(int id)
        {
            var contactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (contactMessage == null)
            {
                return new ErrorResult("Böyle bir mesaj bulunamadı.");
            }

            // Şalter Mantığı (Toggle): Mesaj okunmuşsa (true) okunmadı (false) yapar; okunmamışsa (false) okundu (true) yapar.
            contactMessage.IsRead = !contactMessage.IsRead;

            await _contactMessageRepository.UpdateAsync(contactMessage);
            return new SuccessResult("Mesajın okunma durumu güncellendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingContactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (existingContactMessage == null)
            {
                return new ErrorResult("Silinecek mesaj bulunamadı.");
            }

            existingContactMessage.Status = false;
            await _contactMessageRepository.UpdateAsync(existingContactMessage);
            return new SuccessResult("Mesaj başarıyla silindi.");
        }

        public async Task<IDataResult<List<ContactMessageListDto>>> GetAllAsync()
        {
            var contactMessages = await _contactMessageRepository.GetAllAsync();
            var contactMessageDtos = _mapper.Map<List<ContactMessageListDto>>(contactMessages);
            return new SuccessDataResult<List<ContactMessageListDto>>(contactMessageDtos, "Mesajlar başarıyla listelendi.");
        }

        public async Task<IDataResult<ContactMessageListDto>> GetByIdAsync(int id)
        {
            var contactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (contactMessage == null)
            {
                return new ErrorDataResult<ContactMessageListDto>("Mesaj bulunamadı.");
            }

            var contactMessageDto = _mapper.Map<ContactMessageListDto>(contactMessage);
            return new SuccessDataResult<ContactMessageListDto>(contactMessageDto, "Mesaj başarıyla getirildi.");
        }
    }
}
