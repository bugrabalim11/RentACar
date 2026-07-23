using FluentValidation;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;

namespace RentACar.Business.ValidationRules.UserOperationClaimValidators
{
    public class UserOperationClaimAddValidator : AbstractValidator<UserOperationClaimAddDto>
    {
        public UserOperationClaimAddValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Lütfen geçerli bir kullanıcı seçiniz.");
            RuleFor(x => x.OperationClaimId).GreaterThan(0).WithMessage("Lütfen geçerli bir rütbe seçiniz.");
        }
    }
}
