using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
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

        public async Task<IResult> AddAsync(ContactMessageAddDto contactMessageAddDto)
        {
            await CheckIfUserCanSendMessageAsync(contactMessageAddDto.Email);

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

        private async Task CheckIfUserCanSendMessageAsync(string email)
        {
            bool sendMessage = await _contactMessageRepository.AnyAsync(x => x.Email == email && x.SendDate > DateTime.UtcNow.AddMinutes(-5)); // >= de olabilirdi aynı şey
            if (sendMessage)
            {
                throw new BusinessException("Sistemimizi korumak adına peş peşe mesaj gönderemezsiniz. Lütfen 5 dakika sonra tekrar deneyiniz.");
            }
        }
    }
}
