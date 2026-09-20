using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Areas.Admin.Models.CustomerDtos;
using RentACar.MVC.Areas.Admin.Models.OfficeDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.RentalDtos
{
    public class RentalCreateByAdminViewModel : IRentalDropdownsViewModel
    {
        public List<CustomerResultDto> Customers { get; set; } = new List<CustomerResultDto>();
        public List<CarResultDto> Cars { get; set; } = new List<CarResultDto>();
        public List<OfficeResultDto> Offices { get; set; } = new List<OfficeResultDto>();
        public RentalCreateByAdminDto RentalCreateByAdmin { get; set; } = new RentalCreateByAdminDto();
    }
}
