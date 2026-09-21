using FluentValidation;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;

namespace RentACar.Business.ValidationRules.UserOperationClaimValidators
{
    public class UserOperationClaimCreateValidator : AbstractValidator<UserOperationClaimCreateDto>
    {
        public UserOperationClaimCreateValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Lütfen geçerli bir kullanıcı seçiniz.");
            RuleFor(x => x.OperationClaimId).GreaterThan(0).WithMessage("Lütfen geçerli bir rütbe seçiniz.");
        }
    }
}
