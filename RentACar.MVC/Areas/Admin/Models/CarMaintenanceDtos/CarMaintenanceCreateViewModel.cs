using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.MVC.Areas.Admin.Models.CarDtos;
using RentACar.MVC.Models.Interfaces;

namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceDtos
{
    public class CarMaintenanceCreateViewModel : ICarMaintenanceDropdownViewModel
    {
        // SENIOR NOTU: Bu Yapıcı Metot(Constructor). 
        // CarMaintenanceCreateViewModel sınıfından 'new' kelimesiyle yeni bir nesne üretildiği MİLİSANİYE burası otomatik çalışır.
        public CarMaintenanceCreateViewModel()
        {
            // Büyük kutu (ViewModel) üretildiğinde, içindeki küçük kutunun (Dto) boş (null) kalıp sistemi
            // çökertmemesi için onu da fiziksel olarak inşa ediyoruz.
            CarMaintenanceCreate = new CarMaintenanceCreateDto();
        }

        [ValidateNever]
        public List<CarResultDto> Cars { get; set; } = null!;
        public CarMaintenanceCreateDto CarMaintenanceCreate { get; set; } = null!;
    }
}
