using RentACar.Dtos.CustomerDtos;

namespace RentACar.MVC.Areas.Admin.Models.CustomerViewModels
{
    public class CustomerUpdateByAdminViewModel 
    {
        public CustomerUpdateByAdminDto CustomerUpdateByAdmin { get; set; } = new CustomerUpdateByAdminDto();
    }
}
