using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Dtos.OfficeDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface IRentalDropdownsViewModel
    {
        List<CustomerResultDto> Customers { get; set; }
        List<CarResultDto> Cars { get; set; }
        List<OfficeResultDto> Offices { get; set; }
    }
}
