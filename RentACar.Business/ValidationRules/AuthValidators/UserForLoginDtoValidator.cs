using FluentValidation;
using RentACar.Core.Entities.DTOs;

namespace RentACar.Business.ValidationRules.AuthValidators
{
    public class UserForLoginDtoValidator : AbstractValidator<UserForLoginDto>
    {
        public UserForLoginDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Lütfen e-posta adresinizi giriniz.");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Lütfen şifrenizi giriniz.");
        }
    }
}
