using RentACar.Dtos.UserDtos;

namespace RentACar.MVC.Models.Interfaces
{
    public interface ICustomerDropdownViewModel
    {
        List<UserResultDto> Users { get; set; }
    }
}
