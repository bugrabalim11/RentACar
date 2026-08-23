using FluentValidation;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.Business.ValidationRules.CarMaintenanceValidators
{
    public class CarMaintenanceAddDtoValidator : AbstractValidator<CarMaintenanceAddDto>
    {
        public CarMaintenanceAddDtoValidator()
        {
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Lütfen geçerli bir araç numarası giriniz.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş geçilemez.")
                .MinimumLength(10).WithMessage("Açıklama en az 10 karakter olmalıdır.")
                .MaximumLength(500).WithMessage("Açıklma en fazla 500 karakter olmalıdır.");

            RuleFor(x => x.CheckInTime)
               .NotEmpty()
               .GreaterThanOrEqualTo(DateTime.Today.AddDays(-2))
               .WithMessage("Giriş tarihi, bugünden 2 gün öncesine büyük veya eşit olmalıdır. Yani en fazla 2 gün gecikmeli giriş yapılabilir.");

            RuleFor(x => x.CheckOutTime)
                .GreaterThan(x => x.CheckInTime)
                .When(x => x.CheckOutTime.HasValue)
                .WithMessage("Çıkış tarihi, giriş tarihinden önce olamaz.");
        }
    }
}
