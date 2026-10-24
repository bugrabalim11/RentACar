using RentACar.Dtos.BrandDtos;
using RentACar.Dtos.ColorDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICarDropdownViewModel
    {
        List<BrandResultDto> Brands { get; set; }
        List<ColorResultDto> Colors { get; set; }
    }
}
