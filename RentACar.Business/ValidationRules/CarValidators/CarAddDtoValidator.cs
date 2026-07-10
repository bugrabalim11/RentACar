using FluentValidation;
using RentACar.Dtos.CarDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.ValidationRules.CarValidators
{
    // Sınıfımızın FluentValidation'ın AbstractValidator'ından miras alması gerekiyor
    // İçine de hangi DTO'yu denetleyeceğini (CarAddDto) yazıyoruz.
    // Bool değerlere kural yazmadık çünkü FluentValidation'da bool değerler için genellikle kural yazmaya gerek yoktur.
    public class CarAddDtoValidator : AbstractValidator<CarAddDto>
    {
        public CarAddDtoValidator()
        {
            RuleFor(x => x.BrandId).GreaterThan(0).WithMessage("Lütfen geçerli bir marka seçiniz.");
            RuleFor(x => x.ColorId).GreaterThan(0).WithMessage("Lütfen geçerli bir renk seçiniz.");


            RuleFor(x => x.ModelName)
                 .NotEmpty().WithMessage("Araç model ismi boş bırakılamaz.")
                 .MinimumLength(2).WithMessage("Araç model ismi en az 2 karakter olmalıdır.");

            RuleFor(x => x.Plate)
                .NotEmpty().WithMessage("Araç plakası boş bırakılamaz.")
                .MaximumLength(10).WithMessage("Araç plakası en fazla 10 karakter olmalıdır.");

            RuleFor(x => x.LuggageCapacity)
                .NotEmpty().WithMessage("Bagaj kapasitesi boş geçilemez.");

            RuleFor(x => x.TransmissionType)
                .NotEmpty().WithMessage("Vites tipi boş geçilemez.");  // İleride Enum yapınca burayı değiştireceğiz


            RuleFor(x => x.DailyPrice)
                .NotEmpty().WithMessage("Günlük fiyat alanı boş bırakılamaz.")
                .GreaterThan(0).WithMessage("Günlük fiyat 0'dan büyük olmalıdır.");

            RuleFor(x => x.Kilometer)
                .NotEmpty().WithMessage("Kilometre alanı boş bırakılamaz.")
                .GreaterThanOrEqualTo(0).WithMessage("Kilometre 0'dan küçük olamaz.");

            RuleFor(x => x.DoorCount)
                .NotEmpty().WithMessage("Kapı sayısı alanı boş bırakılamaz.")
                .InclusiveBetween(2, 6).WithMessage("Kapı sayısı 2 ile 6 arasında olmalıdır.");

            RuleFor(x => x.SeatCount)
                .NotEmpty().WithMessage("Koltuk sayısı alanı boş bırakılamaz.")
                .InclusiveBetween(2, 10).WithMessage("Koltuk sayısı 2 ile 10 arasında olmalıdır.");

            RuleFor(x => x.MinDriverAge)
                .NotEmpty().WithMessage("Minimum sürücü yaşı alanı boş bırakılamaz.")
                .GreaterThan(18).WithMessage("Minimum sürücü yaşı 18'den büyük olmalıdır.");
        }
    }
}
