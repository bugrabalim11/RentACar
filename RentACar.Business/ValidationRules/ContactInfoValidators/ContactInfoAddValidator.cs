using FluentValidation;
using RentACar.Dtos.ContactInfoDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.ContactInfoValidators
{
    public class ContactInfoAddValidator : AbstractValidator<ContactInfoAddDto>
    {
        public ContactInfoAddValidator()
        {
            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres bilgisi boş geçilemez.")
                .MinimumLength(10).WithMessage("Adres en az 10 karakter olmalıdır.")
                .MaximumLength(100).WithMessage("Adres en fazla 100 karakter olmalıdır.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("İletişim numarası boş geçilemez.")
                .Length(11).WithMessage("İletişim numarası 11 haneli olmalıdır.")
                .Matches("^[0-9]*$").WithMessage("İletişim numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");
        }
    }
}
