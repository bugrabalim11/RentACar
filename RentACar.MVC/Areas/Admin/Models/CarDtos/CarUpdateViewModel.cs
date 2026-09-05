using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.ColorDtos;

namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class CarUpdateViewModel
    {
        [ValidateNever] 
        public List<BrandResultDto> Brands { get; set; } = null!;

        [ValidateNever]
        public List<ColorResultDto> Colors { get; set; } = null!;
        public CarUpdateDto CarUpdate { get; set; } = null!;
    }
}
