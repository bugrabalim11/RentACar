using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class ContactMessageBusinessRules
    {
        private readonly IContactMessageRepository _contactMessageRepository;

        public ContactMessageBusinessRules(IContactMessageRepository contactMessageRepository)
        {
            _contactMessageRepository = contactMessageRepository;
        }

        public async Task<IResult> CheckIfUserCanSendMessageAsync(string email)
        {
            bool sendMessage = await _contactMessageRepository.AnyAsync(x => x.Email.ToLower() == email && x.CreatedDate > DateTime.UtcNow.AddMinutes(-5)); // >= de olabilirdi aynı şey
            if (sendMessage)
            {
                return new ErrorResult("Sistemimizi korumak adına peş peşe mesaj gönderemezsiniz. Lütfen 5 dakika sonra tekrar deneyiniz.");
            }
            return new SuccessResult();
        }
    }
}
