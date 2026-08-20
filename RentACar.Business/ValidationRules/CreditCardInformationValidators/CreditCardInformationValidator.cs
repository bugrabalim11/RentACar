using FluentValidation;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Business.ValidationRules.CreditCardInformationValidators
{
    public class CreditCardInformationValidator : AbstractValidator<CreditCardInformationDto>
    {
        public CreditCardInformationValidator()
        {
            RuleFor(x => x.CardHolderFullName)
                .NotEmpty().WithMessage("Kart sahibinin adı ve soyadı boş geçilemez.")
                .MinimumLength(3).WithMessage("Kart sahibinin adı ve soyadı en az 3 karakter olmalıdır.")
                .MaximumLength(50).WithMessage("Kart sahibinin adı ve soyadı en fazla 50 karakter olmalıdır.");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Kart numarası boş geçilemez.")
                .Length(16).WithMessage("Kredi kartı numarası 16 haneli olmak zorundadır.")
                .Matches("^[0-9]*$").WithMessage("Kredi kartı numarası sadece rakamlardan oluşmalıdır.");

            RuleFor(x => x.ExpireYear)
                .NotEmpty().WithMessage("Kart geçerlilik yılı boş geçilemez.")
                .GreaterThanOrEqualTo(DateTime.UtcNow.Year).WithMessage("Kartınızın süresi dolmuş, geçerlilik yılı bu yıldan küçük olamaz.")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 15).WithMessage("Geçerlilik yılı çok ileri bir tarih olamaz.");

            RuleFor(x => x.ExpireMonth)
                .NotEmpty().WithMessage("Kart geçerlilik ayı boş geçilemez.")
                .InclusiveBetween(1, 12).WithMessage("Ay bilgisi 1 ile 12 arasında olmalıdır.");
                When(x => x.ExpireYear == DateTime.UtcNow.Year, () =>
                {
                    RuleFor(x => x.ExpireMonth)
                        .GreaterThanOrEqualTo(DateTime.UtcNow.Month)
                        .WithMessage("Kartınızın süresi dolmuş, geçerlilik ayı bu aydan küçük olamaz.");
                });

            RuleFor(x => x.Cvv)
                .NotEmpty().WithMessage("Cvv boş geçilemez.")
                .Length(3).WithMessage("Cvv 3 rakamdan oluşmalıdır.")
                .Matches("^[0-9]*$").WithMessage("Cvv sadece rakamlardan oluşmalıdır.");
        }
    }
}
