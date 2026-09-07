using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.ColorDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICarDropdownViewModel
    {
        public List<BrandResultDto> Brands { get; set; }
        public List<ColorResultDto> Colors { get; set; }
    }
}
