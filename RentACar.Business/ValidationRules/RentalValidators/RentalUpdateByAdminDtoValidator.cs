using FluentValidation;
using RentACar.Dtos.RentalDtos;

namespace RentACar.Business.ValidationRules.RentalValidators
{
    public class RentalUpdateByAdminDtoValidator : AbstractValidator<RentalUpdateByAdminDto>
    {
        public RentalUpdateByAdminDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen geçerli bir kiralama seçiniz.");
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Lütfen geçerli bir araç seçiniz.");
            RuleFor(x => x.PickUpOfficeId).GreaterThan(0).WithMessage("Lütfen geçerli bir şube seçiniz.");
            RuleFor(x => x.DropOffOfficeId).GreaterThan(0).WithMessage("Lütfen geçerli bir şube seçiniz.");

            RuleFor(x => x.RentDate)
                .NotEmpty().WithMessage("Kiralama tarihi boş geçilemez.")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Kiralama tarihi bugünün tarihinden önce olamaz!");

            RuleFor(x => x.ReturnDate)
                .GreaterThan(x => x.RentDate)   
                .When(x => x.ReturnDate.HasValue) 
                .WithMessage("İade tarihi, kiralama tarihinden önce olamaz!");
        }
    }
}
