using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ContactMessageDtos;
using RentACar.Entities.Concrete;

namespace RentACar.Business.Concrete
{
    public class ContactMessageManager : IContactMessageService
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IMapper _mapper;

        public ContactMessageManager(IContactMessageRepository contactMessageRepository, IMapper mapper)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(ContactMessageCreateDto contactMessageAddDto)
        {
            // 1. TEMİZLİK
            contactMessageAddDto.Email = contactMessageAddDto.Email.Trim().ToLower();

            // 2. RATE LIMITING (SPAM KORUMASI): Aynı adam peş peşe mesaj atıp sistemi yoramasın.
            IResult? result = BusinessRules.Run(await CheckIfUserCanSendMessageAsync(contactMessageAddDto.Email));
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Sistemimizi korumak adına peş peşe mesaj gönderemezsiniz. Lütfen 5 dakika sonra tekrar deneyiniz!");
            }

            // 3. MÜHÜRLEME: Tarihi ve Okunma durumunu sistem manuel basar, müşteriye güvenilmez.
            var contactMessage = _mapper.Map<ContactMessage>(contactMessageAddDto);
            contactMessage.SendDate = DateTime.UtcNow;
            contactMessage.IsRead = false;

            await _contactMessageRepository.AddAsync(contactMessage);
            return new SuccessResult("Mesajınız başarıyla gönderildi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingContactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (existingContactMessage == null)
            {
                throw new BusinessException("Silinecek mesaj bulunamadı.");
            }

            existingContactMessage.IsDeleted = true;
            existingContactMessage.DeletedDate = DateTime.UtcNow;
            await _contactMessageRepository.UpdateAsync(existingContactMessage);
            return new SuccessResult("Mesaj başarıyla silindi.");
        }

        public async Task<IDataResult<List<ContactMessageResultDto>>> GetAllAsync()
        {
            var contactMessages = await _contactMessageRepository.GetAllAsync();
            var contactMessageDtos = _mapper.Map<List<ContactMessageResultDto>>(contactMessages);
            return new SuccessDataResult<List<ContactMessageResultDto>>(contactMessageDtos, "Mesajlar başarıyla listelendi.");
        }

        public async Task<IDataResult<ContactMessageResultDto>> GetByIdAsync(int id)
        {
            var contactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (contactMessage == null)
            {
                throw new BusinessException("Mesaj bulunamadı.");
            }

            var contactMessageDto = _mapper.Map<ContactMessageResultDto>(contactMessage);
            return new SuccessDataResult<ContactMessageResultDto>(contactMessageDto, "Mesaj başarıyla getirildi.");
        }

        // TASK-BASED UPDATE 2: Tek Yönlü İşlem (Idempotent)
        public async Task<IResult> MarkAsReadAsync(int id)
        {
            var contactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (contactMessage == null)
            {
                throw new BusinessException("Mesaj bulunamadı.");
            }

            // Sadece okunmamışsa veritabanına gidip günceller (Performans dostu).
            if (!contactMessage.IsRead)
            {
                contactMessage.IsRead = true;
                await _contactMessageRepository.UpdateAsync(contactMessage);
            }
            return new SuccessResult();
        }

        // --- İÇ RAPORLAMA MERKEZİ ---
        private async Task<IResult> CheckIfUserCanSendMessageAsync(string email)
        {
            // Zaman Yolcusu Kontrolü: Şu anki saatten (UtcNow) 5 dakika öncesine (-5) gidiyoruz.
            // Eğer adamın son mesaj tarihi bu 5 dakikalık pencerenin içindeyse (büyükse), true döner ve adamı bloklarız.
            bool sendMessage = await _contactMessageRepository.AnyAsync(x => x.Email.ToLower() == email && x.SendDate > DateTime.UtcNow.AddMinutes(-5)); // >= de olabilirdi aynı şey
            if (sendMessage)
            {
                return new ErrorResult("Sistemimizi korumak adına peş peşe mesaj gönderemezsiniz. Lütfen 5 dakika sonra tekrar deneyiniz.");
            }
            return new SuccessResult();
        }
    }
}
