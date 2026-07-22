using FluentValidation;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;

namespace RentACar.Business.ValidationRules.OperationClaimValidators
{
    public class OperationClaimUpdateDtoValidator : AbstractValidator<OperationClaimUpdateDto>
    {
        public OperationClaimUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen geçerli bir statü seçiniz.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Statü boş geçilemez.")
                .MinimumLength(2).WithMessage("Statü en az 2 karakter olmalıdır.")
                .MaximumLength(10).WithMessage("Statü en fazla 10 karakter olmalıdır.");
        }
    }

}
