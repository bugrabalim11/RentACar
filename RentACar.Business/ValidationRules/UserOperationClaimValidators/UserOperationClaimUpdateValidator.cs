using FluentValidation;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;

namespace RentACar.Business.ValidationRules.UserOperationClaimValidators
{
    public class UserOperationClaimUpdateValidator : AbstractValidator<UserOperationClaimUpdateDto>
    {
        public UserOperationClaimUpdateValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen güncellenecek yetki atamasını seçiniz.");
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Lütfen geçerli bir kullanıcı seçiniz.");
            RuleFor(x => x.OperationClaimId).GreaterThan(0).WithMessage("Lütfen geçerli bir rütbe seçiniz.");
        }
    }
}
