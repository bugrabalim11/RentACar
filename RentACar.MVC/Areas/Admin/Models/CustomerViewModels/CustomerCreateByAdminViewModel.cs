using RentACar.Dtos.CustomerDtos;

namespace RentACar.MVC.Areas.Admin.Models.CustomerViewModels
{
    public class CustomerCreateByAdminViewModel 
    {
        public CustomerCreateByAdminDto CustomerCreateByAdmin { get; set; } = new CustomerCreateByAdminDto();
    }
}
