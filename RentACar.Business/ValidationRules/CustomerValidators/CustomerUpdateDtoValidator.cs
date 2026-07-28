using FluentValidation;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.ValidationRules.CustomerValidators
{
    public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
    {
        public CustomerUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen güncellenecek geçerli bir müşteri seçiniz.");

            RuleFor(x => x.NationalIdentity)
                    .NotEmpty().WithMessage("TC kimlik numarası zorunludur.")
                    .Length(11).WithMessage("TC kimlik numarası tam olarak 11 haneli olmak zorundadır.")
                    .Matches("^[0-9]*$").WithMessage("TC kimlik numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.DrivingLicenseYear)
                    .GreaterThan(1950).WithMessage("Geçerli bir ehliyet yılı giriniz.")
                    .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Ehliyet yılı geleceketeki bir yıl olamaz.");
        }
    }
}
