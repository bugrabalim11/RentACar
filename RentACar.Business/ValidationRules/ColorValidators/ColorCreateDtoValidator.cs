using FluentValidation;
using RentACar.Dtos.ColorDtos;

namespace RentACar.Business.ValidationRules.ColorValidators
{
    public class ColorCreateDtoValidator : AbstractValidator<ColorCreateDto>
    {
        public ColorCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Renk ismi boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Renk ismi en az 2 karakter olmalıdır.")
                .MaximumLength(20).WithMessage("Renk ismi 20 karakteri geçemez.");
        }
    }
}
