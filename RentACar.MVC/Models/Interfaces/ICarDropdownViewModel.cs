using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.ColorDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICarDropdownViewModel
    {
        List<BrandResultDto> Brands { get; set; }
        List<ColorResultDto> Colors { get; set; }
    }
}
