using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ContactInfoDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class AdminContactInfosManager : IAdminContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;
        private readonly ContactInfoBusinessRules _contactInfoBusinessRules;

        public AdminContactInfosManager(IContactInfoRepository contactInfoRepository, IMapper mapper, ContactInfoBusinessRules contactInfoBusinessRules)
        {
            _contactInfoRepository = contactInfoRepository;
            _mapper = mapper;
            _contactInfoBusinessRules = contactInfoBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(ContactInfoCreateDto contactInfoAddDto)
        {
            IResult? result = BusinessRules.Run(
                await _contactInfoBusinessRules.CheckIfContactInfoAlreadyExistsAsync()
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            var contactInfo = _mapper.Map<ContactInfo>(contactInfoAddDto);
            await _contactInfoRepository.AddAsync(contactInfo);
            return new SuccessDataResult<int>(contactInfo.Id, "İletişim bilgisi başarıyla eklendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingContactInfo = await _contactInfoRepository.GetAsync(x => x.Id == id);
            if (existingContactInfo == null)
            {
                throw new BusinessException("Silinecek iletişim bilgisi bulunamadı.");
            }

            await _contactInfoRepository.DeleteAsync(existingContactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla silindi.");
        }

        public async Task<IResult> UpdateAsync(ContactInfoUpdateDto contactInfoUpdateDto)
        {
            var existingContactInfo = await _contactInfoRepository.GetAsync(x => x.Id == contactInfoUpdateDto.Id);
            if (existingContactInfo == null)
            {
                throw new BusinessException("Güncellenecek iletişim bilgisi bulunamadı.");
            }

            _mapper.Map(contactInfoUpdateDto, existingContactInfo);
            await _contactInfoRepository.UpdateAsync(existingContactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla güncellendi.");
        }
    }
}
