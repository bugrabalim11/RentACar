using FluentValidation;
using RentACar.Dtos.ContactMessageDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.ContactMessageValidators
{
    public class ContactMessageAddDtoValidator : AbstractValidator<ContactMessageAddDto>
    {
        public ContactMessageAddDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(2).WithMessage("İsim en az 2 karakter olmalıdır.");

            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta boş geçilemez.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Subject).NotEmpty()
                .MinimumLength(3).WithMessage("Konu en az 3 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Konu en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.Message).NotEmpty().MinimumLength(10).WithMessage("Mesaj en az 10 karakter olmalıdır.");
        }
    }
}
