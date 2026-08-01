using FluentValidation;
using Microsoft.AspNetCore.Http;
using RentACar.Dtos.CarImageDtos;

namespace RentACar.Business.ValidationRules.CarImageValidayors
{
    public class CarImageUpdateDtoValidator : AbstractValidator<CarImageUpdateDto>
    {
        public CarImageUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Lütfen geçerli bir resim seçiniz.");
            RuleFor(x => x.CarId).GreaterThan(0).WithMessage("Lütfen geçerli bir araç seçiniz.");
            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("Lütfen bir resim dosyası seçiniz.")
                .NotNull().WithMessage("Lütfen bir resim dosyası seçiniz.")
                .Must(IsImageValid).WithMessage("Sadece .jpg, .jpeg veya .png formatında resim yükleyebilirsiniz!")
                .Must(IsFileSizeValid).WithMessage("Resim boyutu 5 MB'den büyük olamaz!");
        }

        private bool IsImageValid(IFormFile arg)
        {
            if (arg == null) return false;

            return arg.ContentType == "image/jpeg" || arg.ContentType == "image/png" || arg.ContentType == "image/jpg";
        }

        private bool IsFileSizeValid(IFormFile arg)
        {
            if (arg == null) return false;

            return arg.Length <= 5 * 1024 * 1024; // 5 MB
        }
    }
}
