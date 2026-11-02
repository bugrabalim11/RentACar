using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
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
        private readonly ContactMessageBusinessRules _contactMessageBusinessRules;

        public ContactMessageManager(IContactMessageRepository contactMessageRepository, IMapper mapper, ContactMessageBusinessRules contactMessageBusinessRules)
        {
            _contactMessageRepository = contactMessageRepository;
            _mapper = mapper;
            _contactMessageBusinessRules = contactMessageBusinessRules;
        }

        public async Task<IDataResult<int>> AddAsync(ContactMessageCreateDto contactMessageAddDto)
        {
            contactMessageAddDto.Email = contactMessageAddDto.Email.Trim().ToLower();
            IResult? result = BusinessRules.Run(
                await _contactMessageBusinessRules.CheckIfUserCanSendMessageAsync(contactMessageAddDto.Email)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "Sistemimizi korumak adına peş peşe mesaj gönderemezsiniz. Lütfen 5 dakika sonra tekrar deneyiniz!");
            }
            var contactMessage = _mapper.Map<ContactMessage>(contactMessageAddDto);
            contactMessage.IsRead = false;
            await _contactMessageRepository.AddAsync(contactMessage);
            return new SuccessDataResult<int>(contactMessage.Id, "Mesajınız başarıyla gönderildi.");
        }
    }
}
