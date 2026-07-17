using FluentValidation;
using RentACar.Dtos.RentalDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.RentalValidators
{
    public class RentalAddDtoValidator : AbstractValidator<RentalAddDto>
    {
        public RentalAddDtoValidator()
        {
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Lütfen geçerli bir araç seçiniz.");
            RuleFor(x => x.CustomerId).GreaterThan(0).WithMessage("Lütfen geçerli bir müşteri seçiniz.");
            RuleFor(x => x.PickUpOfficeId).GreaterThan(0).WithMessage("Lütfen geçerli bir şube seçiniz.");
            RuleFor(x => x.DropOffOfficeId).GreaterThan(0).WithMessage("Lütfen geçerli bir şube seçiniz.");

            RuleFor(x => x.RentDate).NotEmpty();
            RuleFor(x => x.ReturnDate)
                .GreaterThan(x => x.RentDate)   // İade tarihi, kiralama tarihinden büyük olmalı
                .When(x => x.ReturnDate.HasValue) // SADECE iade tarihi girilmişse (null değilse) bu kuralı çalıştır
                .WithMessage("İade tarihi, kiralama tarihinden önce olamaz!");
        }
    }
}
