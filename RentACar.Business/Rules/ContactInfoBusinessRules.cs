using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Rules
{
    public class ContactInfoBusinessRules
    {
        private readonly IContactInfoRepository _contactInfoRepository;

        public ContactInfoBusinessRules(IContactInfoRepository contactInfoRepository)
        {
            _contactInfoRepository = contactInfoRepository;
        }

        public async Task<IResult> CheckIfContactInfoAlreadyExistsAsync()
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
