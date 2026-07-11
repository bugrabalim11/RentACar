using FluentValidation;
using RentACar.Dtos.ColorDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.ColorValidators
{
    public class ColorAddDtoValidator : AbstractValidator<ColorAddDto>
    {
        public ColorAddDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Renk ismi boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Renk ismi en az 2 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Renk ismi 20 karakteri geçemez.");
        }
    }
}
