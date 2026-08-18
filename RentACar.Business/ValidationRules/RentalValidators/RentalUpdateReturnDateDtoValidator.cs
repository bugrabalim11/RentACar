using FluentValidation;
using RentACar.Dtos.RentalDtos;

namespace RentACar.Business.ValidationRules.RentalValidators
{
    public class RentalUpdateReturnDateDtoValidator : AbstractValidator<RentalUpdateReturnDateDto>
    {
        public RentalUpdateReturnDateDtoValidator()
        {
            RuleFor(x => x.ReturnDate).NotEmpty().WithMessage("Araç teslim tarihi boş geçilemez.");
        }
    }
}
