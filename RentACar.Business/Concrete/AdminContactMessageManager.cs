using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.ContactMessageDtos;

namespace RentACar.Business.Concrete
{
    public class AdminContactMessageManager : IAdminContactMessageService
    {
        private readonly IContactMessageRepository _contactMessageRepository;
        private readonly IMapper _mapper;

        public AdminContactMessageManager(IContactMessageRepository contactMessageRepository, IMapper mapper)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
        }

        public async Task<IDataResult<List<ContactMessageResultDto>>> GetAllAsync()
        {
            var contactMessages = await _contactMessageRepository.GetAllAsync();
            var sortedContactMessages = contactMessages.OrderByDescending(x => x.CreatedDate).ToList();
            var contactMessageDtos = _mapper.Map<List<ContactMessageResultDto>>(sortedContactMessages);
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

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingContactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (existingContactMessage == null)
            {
                throw new BusinessException("Silinecek mesaj bulunamadı.");
            }

            await _contactMessageRepository.DeleteAsync(existingContactMessage);
            return new SuccessResult("Mesaj başarıyla silindi.");
        }

        public async Task<IResult> MarkAsReadAsync(int id)
        {
            var contactMessage = await _contactMessageRepository.GetAsync(x => x.Id == id);
            if (contactMessage == null)
            {
                throw new BusinessException("Mesaj bulunamadı.");
            }
            if (!contactMessage.IsRead)
            {
                contactMessage.IsRead = true;
                await _contactMessageRepository.UpdateAsync(contactMessage);
            }
            return new SuccessResult();
        }
    }
}
