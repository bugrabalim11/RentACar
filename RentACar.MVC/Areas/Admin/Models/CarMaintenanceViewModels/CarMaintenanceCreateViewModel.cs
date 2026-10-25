using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using RentACar.Dtos.CarDtos;
using RentACar.Dtos.CarMaintenanceDtos;

namespace RentACar.MVC.Areas.Admin.Models.CarMaintenanceViewModels
{
    public class CarMaintenanceCreateViewModel 
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
        public CarMaintenanceCreateDto CarMaintenanceCreate { get; set; } = null!;
    }
}
