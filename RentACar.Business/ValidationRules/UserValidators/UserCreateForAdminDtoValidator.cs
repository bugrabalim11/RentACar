using FluentValidation;
using RentACar.Core.Entities.DTOs.UserDtos;

namespace RentACar.Business.ValidationRules.UserValidators
{
    public class UserCreateForAdminDtoValidator : AbstractValidator<UserCreateByAdminDto>
    {
        public UserCreateForAdminDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Kullanıcı adı boş geçilemez.")
                .MinimumLength(2).WithMessage("Kullanıcı adı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı adı en fazla 50 karakter olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Kullanıcı soyadı boş geçilemez.")
                .MinimumLength(2).WithMessage("Kullanıcı soyadı en az 2 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kullanıcı soyadı en fazla 50 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre alanı boş geçilemez.")
                .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
                .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
                .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
                .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.");
        }
    }
}
