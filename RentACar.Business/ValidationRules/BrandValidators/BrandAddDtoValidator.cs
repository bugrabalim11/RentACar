using FluentValidation;
using RentACar.Dtos.BrandDtos;

namespace RentACar.Business.ValidationRules.BrandValidators
{
    public class BrandAddDtoValidator : AbstractValidator<BrandAddDto>
    {
        public BrandAddDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Araç markası boş bırakılamaz.")
                .MinimumLength(2).WithMessage("Araç markası en az 2 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Araç markası en fazla 30 karakter olmalıdır.");
        }
    }
}
