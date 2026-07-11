using FluentValidation;
using RentACar.Dtos.BrandDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.BrandValidators
{
    public class BrandUpdateDtoValidator : AbstractValidator<BrandUpdateDto>
    {
        public BrandUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen güncellencek geçerli bir marka seçiniz.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Araç markası boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Araç markası en az 2 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Araç markası en fazla 30 karakter olmalıdır.");
        }
    }
}
