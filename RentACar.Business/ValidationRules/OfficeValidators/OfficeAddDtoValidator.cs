using FluentValidation;
using RentACar.Dtos.OfficeDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.OfficeValidators
{
    public class OfficeAddDtoValidator : AbstractValidator<OfficeAddDto>
    {
        public OfficeAddDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ofis ismi boş geçilemez.")
                .MinimumLength(2).WithMessage("Ofis ismi en az 2 karakter olmalıdır.")
                .MaximumLength(30).WithMessage("Ofis ismi en fazla 30 karakter olmalıdır.");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir ismi boş geçilemez.")
                .MinimumLength(2).WithMessage("Şehir ismi en az 2 karakter olmalıdır.")
                .MaximumLength(15).WithMessage("Şehir ismi en fazla 15 karakter olmalıdır.");

            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("İletişim numarası boş geçilemez.")
                .Length(11).WithMessage("İletişim numarası 11 haneli olmalıdır.")
                .Matches("^[0-9]*$").WithMessage("İletişim numarası sadece rakamlardan oluşmalıdır.");
        }
    }
}
