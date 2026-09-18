using RentACar.MVC.Areas.Admin.Models.UserDtos;

namespace RentACar.MVC.Models.Interfaces
{
    // TODO Dropdownları  Select2 JavaScript Kütühanesi ile Arama Kutucuğu yap
    public interface ICustomerDropdownViewModel
    {
        List<UserResultDto> Users { get; set; }
    }
}
