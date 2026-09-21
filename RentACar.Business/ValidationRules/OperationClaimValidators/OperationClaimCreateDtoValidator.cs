using FluentValidation;
using RentACar.Core.Entities.DTOs.OperationClaimDtos;

namespace RentACar.Business.ValidationRules.OperationClaimValidators
{
    public class OperationClaimCreateDtoValidator : AbstractValidator<OperationClaimCreateDto>
    {
        public OperationClaimCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Statü boş geçilemez.")
                .MinimumLength(2).WithMessage("Statü en az 2 karakter olmalıdır.")
                .MaximumLength(10).WithMessage("Statü en fazla 10 karakter olmalıdır.");
        }
    }
}
