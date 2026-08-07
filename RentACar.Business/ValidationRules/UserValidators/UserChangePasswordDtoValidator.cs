using FluentValidation;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.ValidationRules.UserValidators
{
    public class UserChangePasswordDtoValidator : AbstractValidator<UserChangePasswordDto>
    {
        public UserChangePasswordDtoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Lütfen geçerli bir kullanıcı seçinizk.");

            RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Eski şifre boş olamaz.");

            RuleFor(x=>x.NewPassword)
                .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
                .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
                .Matches("[A-Z]").WithMessage("Yeni şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Yeni şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Yeni şifre en az bir rakam içermelidir.");
        }
    }
}
