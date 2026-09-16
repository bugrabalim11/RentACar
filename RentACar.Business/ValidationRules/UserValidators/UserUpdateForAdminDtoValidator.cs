using FluentValidation;
using RentACar.Core.Entities.DTOs.UserDtos;

namespace RentACar.Business.ValidationRules.UserValidators
{
    public class UserUpdateForAdminDtoValidator : AbstractValidator<UserUpdateForAdminDto>
    {
        public UserUpdateForAdminDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen gecçerli bir yetki seçiniz.");
            RuleFor(x => x.OperationClaimId).GreaterThan(0).WithMessage("Lütfen gecçerli bir yetki seçiniz.");

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
        }
    }
}
