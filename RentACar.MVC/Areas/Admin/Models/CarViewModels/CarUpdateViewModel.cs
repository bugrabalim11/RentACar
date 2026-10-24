using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.Dtos.BrandDtos;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.ColorDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.CarViewModels
{
    public class CarUpdateViewModel : ICarDropdownViewModel
    {
        [ValidateNever]
        public List<BrandResultDto> Brands { get; set; } = null!;

        [ValidateNever]
        public List<ColorResultDto> Colors { get; set; } = null!;
        public CarUpdateDto CarUpdate { get; set; } = null!;
    }
}
