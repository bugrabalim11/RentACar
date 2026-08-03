using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ContactInfoDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class ContactInfoManager : IContactInfoService
    {
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IMapper _mapper;

        public ContactInfoManager(IContactInfoRepository contactInfoRepository, IMapper mapper)
        {
            _contactInfoRepository = contactInfoRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(ContactInfoAddDto contactInfoAddDto)
        {
            IResult? result = BusinessRules.Run(await CheckIfContactInfoAlreadyExistsAsync());
            if (result != null)
            {
                return result;
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

            existingContactInfo.IsDeleted = true;
            existingContactInfo.DeletedDate = DateTime.UtcNow;
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


        private async Task<IResult> CheckIfContactInfoAlreadyExistsAsync()
        {
            bool isExist = await _contactInfoRepository.AnyAsync(x => !x.IsDeleted);
            if (isExist)
            {
                return new ErrorResult("Sisteme zaten bir iletişim bilgisi kayıtlı! Lütfen mevcut kaydı güncelleyiniz.");
            }
            return new SuccessResult();
        }
    }
}
