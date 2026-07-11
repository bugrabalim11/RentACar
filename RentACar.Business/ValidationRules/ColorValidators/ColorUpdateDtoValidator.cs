using FluentValidation;
using RentACar.Dtos.ColorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.ColorValidators
{
    public class ColorUpdateDtoValidator : AbstractValidator<ColorUpdateDto>
    {
        public ColorUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen güncellencek geçerli bir renk seçiniz.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Renk ismi boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Renk ismi en az 2 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Renk ismi en fazla 20 karakter olmalıdır.");
        }
    }
}
