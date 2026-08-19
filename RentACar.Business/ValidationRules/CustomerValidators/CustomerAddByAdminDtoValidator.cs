using FluentValidation;
using RentACar.Dtos.CustomerDtos;

namespace RentACar.Business.ValidationRules.CustomerValidators
{
    public class CustomerAddByAdminDtoValidator : AbstractValidator<CustomerAddByAdminDto>
    {
        public CustomerAddByAdminDtoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0).WithMessage("Lütfen geçerli bir kullanıcı ID'si giriniz.");

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
