using FluentValidation;
using RentACar.Business.ValidationRules.CreditCardInformationValidators;
using RentACar.Dtos.RentalDtos;

namespace RentACar.Business.ValidationRules.RentalValidators
{
    public class RentalAddByAdminDtoValidator : AbstractValidator<RentalAddByAdminDto>
    {
        public RentalAddByAdminDtoValidator()
        {
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Lütfen geçerli bir müşteri ID'si giriniz.");
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Lütfen geçerli bir araç seçiniz.");
            RuleFor(x => x.PickUpOfficeId).GreaterThan(0).WithMessage("Lütfen geçerli bir şube seçiniz.");
            RuleFor(x => x.DropOffOfficeId).GreaterThan(0).WithMessage("Lütfen geçerli bir şube seçiniz.");

            RuleFor(x => x.RentDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Kiralama tarihi bugünün tarihinden önce olamaz!");

            RuleFor(x => x.ReturnDate)
                .GreaterThan(x => x.RentDate)
                .WithMessage("İade tarihi, kiralama tarihinden önce olamaz!");

            RuleFor(x => x.CreditCardInformation).SetValidator(new CreditCardInformationValidator());
        }
    }
}
