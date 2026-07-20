using FluentValidation;
using RentACar.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.UserValidators
{
    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen geçerli bir kullanıcı seçiniz.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Kullanıcı ismi boş geçilemez.")
                .MinimumLength(2).WithMessage("Kullanıcı ismi en az 2 karakter olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Kullanıcı soyismi boş geçilemez.")
                .MinimumLength(2).WithMessage("Kullancı soyismi en az 2 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");
        }
    }
}
