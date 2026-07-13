using FluentValidation;
using RentACar.Dtos.CustomerDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.CustomerValidators
{
    public class CustomerAddDtoValidator : AbstractValidator<CustomerAddDto>
    {
        public CustomerAddDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Müşteri adı boş geçilemez.")
                .MinimumLength(2).WithMessage("Müşteri adı en az 2 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Müşteri adı en fazla 20 karakter olmalıdır.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Müşteri soyadı boş geçilemez.")
                .MinimumLength(2).WithMessage("Müşteri soyadı en az 2 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Müşteri soyadı en fazla 20 karakter olmalıdır.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş geçilemez.")
                .EmailAddress().WithMessage("Lütfen geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.NationalIdentity)
                .NotEmpty().WithMessage("TC kimlik numarası zorunludur.")
                .Length(11).WithMessage("TC kimlik numarası tam olarak 11 haneli olmak zorundadır.")
                .Matches("^[0-9]*$").WithMessage("TC kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(X => X.DrivinglicenseYear)
                .GreaterThan(1950).WithMessage("Geçerli bir ehliyet yılı giriniz.")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Ehliyet yılı geleceketeki bir yıl olamaz.");
        }
    }
}
