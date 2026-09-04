using RentACar.MVC.Areas.Admin.Models.BrandDtos;
using RentACar.MVC.Areas.Admin.Models.ColorDtos;

namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    // VİEWMODEL MANTIĞI (GARSONUN TEPSİSİ):
    // HTML sayfaları (View) kural gereği kendisine sadece TEK BİR model (kargo kolisi) kabul eder.
    // Yeni araç ekleme sayfasında hem boş bir sipariş formuna (CarCreateDto), 
    // hem de Dropdown (Açılır liste) için API'den gelecek Marka ve Renk listelerine ihtiyacımız var.
    // Bu yüzden hepsini tek bir tepside toplayıp sayfaya güvenle yollamak için bu ViewModel'i oluşturduk.
    public class CarCreateViewModel
    {
        public List<BrandResultDto> Brands { get; set; } = null!;
        public List<ColorResultDto> Colors { get; set; } = null!;

        // Adminin sayfada doldurup mutfağa geri fırlatacağı "Boş Sipariş Fişi"
        public CarCreateDto CarCreate { get; set; } = null!;
    }
}
