using RentACar.MVC.Areas.Admin.Models.UserDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICustomerDropdownViewModel
    {
        List<UserResultDto> Users { get; set; }
    }
}
