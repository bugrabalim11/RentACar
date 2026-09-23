using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
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

        public async Task<IResult> AddAsync(ContactInfoCreateDto contactInfoAddDto)
        {
            // SINGLETON KURALI: Sistemde sadece 1 adet aktif iletişim bilgisi olabilir!
            IResult? result = BusinessRules.Run(await CheckIfContactInfoAlreadyExistsAsync());
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Zaten bir tane iletişim bilgisi kayıtlı! Başka bir tane daha ekleyemezsiniz.");
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
                throw new BusinessException("Silinecek iletişim bilgisi bulunamadı.");
            }

            // SOFT DELETE: Çöpe atıyoruz, veritabanından tamamen silmiyoruz.
            existingContactInfo.IsDeleted = true;
            existingContactInfo.DeletedDate = DateTime.UtcNow;
            await _contactInfoRepository.UpdateAsync(existingContactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla silindi.");
        }

        public async Task<IDataResult<List<ContactInfoResultDto>>> GetAllAsync()
        {
            var contactInfos = await _contactInfoRepository.GetAllAsync();
            var contactInfoDtos = _mapper.Map<List<ContactInfoResultDto>>(contactInfos);
            return new SuccessDataResult<List<ContactInfoResultDto>>(contactInfoDtos, "İletişim bilgileri başarıyla listelendi.");
        }

        public async Task<IDataResult<ContactInfoResultDto>> GetByIdAsync(int id)
        {
            var contactInfo = await _contactInfoRepository.GetAsync(x => x.Id == id);
            if (contactInfo == null)
            {
                // BUM! Eski ErrorDataResult formları yakıldı, Kırmızı Alarm devrede!
                throw new BusinessException("İletişim bilgisi bulunamadı.");
            }

            var contactInfoDto = _mapper.Map<ContactInfoResultDto>(contactInfo);
            return new SuccessDataResult<ContactInfoResultDto>(contactInfoDto, "İletişim bilgisi başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(ContactInfoUpdateDto contactInfoUpdateDto)
        {
            var existingContactInfo = await _contactInfoRepository.GetAsync(x => x.Id == contactInfoUpdateDto.Id);
            if (existingContactInfo == null)
            {
                throw new BusinessException("Güncellenecek iletişim bilgisi bulunamadı.");
            }

            // TERCÜMAN (Mapper): DTO'daki yeni bilgileri, veritabanındaki mevcut nesnenin üzerine direkt yazar.
            _mapper.Map(contactInfoUpdateDto, existingContactInfo);
            await _contactInfoRepository.UpdateAsync(existingContactInfo);
            return new SuccessResult("İletişim bilgisi başarıyla güncellendi.");
        }

        // --- İÇ RAPORLAMA MERKEZİ ---
        private async Task<IResult> CheckIfContactInfoAlreadyExistsAsync()
        {
            // Veritabanında silinmemiş (aktif) bir iletişim bilgisi var mı diye sorar.
            bool isExist = await _contactInfoRepository.AnyAsync(x => !x.IsDeleted);
            if (isExist)
            {
                return new ErrorResult("Sisteme zaten bir iletişim bilgisi kayıtlı! Lütfen mevcut kaydı güncelleyiniz.");
            }
            return new SuccessResult();
        }
    }
}