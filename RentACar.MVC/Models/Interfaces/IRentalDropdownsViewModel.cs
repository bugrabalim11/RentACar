using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CustomerDtos;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface IRentalDropdownsViewModel
    {
        List<CustomerResultDto> Customers { get; set; }
        List<CarResultDto> Cars { get; set; }
        List<OfficeResultDto> Offices { get; set; }
    }
}
