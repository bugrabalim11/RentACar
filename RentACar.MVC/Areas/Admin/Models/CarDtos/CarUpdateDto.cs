using RentACar.MVC.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RentACar.MVC.Areas.Admin.Models.CarDtos
{
    public class CarUpdateDto
    {
        [Required(ErrorMessage = "Lütfen bir araç seçiniz!")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen geçerli bir araç seçiniz!")]
        public int? Id { get; set; }

        [Required(ErrorMessage = "Lütfen bir marka seçiniz!")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir marka seçiniz!")]
        public int? BrandId { get; set; }

        [Required(ErrorMessage = "Lütfen bir renk seçiniz!")]
        [Range(1, int.MaxValue, ErrorMessage = "Lütfen bir renk seçiniz!")]
        public int? ColorId { get; set; }

        [Required(ErrorMessage = "Araç model ismi boş bırakılamaz!")]
        [MinLength(2, ErrorMessage = "Araç model ismi en az 2 karakter olmalıdır!")]
        public string ModelName { get; set; } = null!;

        [Required(ErrorMessage = "Kilometre alanı boş geçilemez!")]
        [Range(0, int.MaxValue, ErrorMessage = "Kilometre 0'dan büyük olmalıdır!")]
        public int? Kilometer { get; set; }

        [Required(ErrorMessage = "Araç plakası boş bırakılamaz!")]
        [MaxLength(10, ErrorMessage = "Araç plakası en fazla 10 karakter olmalıdır!")]
        public string Plate { get; set; } = null!;

        [Required(ErrorMessage = "Günlük fiyat alanı boş geçilemez!")]
        [Range(0, int.MaxValue, ErrorMessage = "Günlük fiyat 0'dan büyük olmalıdır!")]
        public decimal? DailyPrice { get; set; }

        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Kapı sayısı alanı boş bırakılamaz!")]
        [Range(2, 6, ErrorMessage = "Kapı sayısı 2 ile 6 arasında olmalıdır!")]
        public int? DoorCount { get; set; }

        [Required(ErrorMessage = "Koltuk sayısı alanı boş bırakılamaz!")]
        [Range(2, 10, ErrorMessage = "Koltuk sayısı 2 ile 10 arasında olmalıdır!")]
        public int? SeatCount { get; set; }

        [Required(ErrorMessage = "Minimum sürücü yaşı alanı boş bırakılamaz!")]
        [Range(18, int.MaxValue, ErrorMessage = "Minimum sürücü yaşı 18'den küçük olamaz!")]
        public int? MinDriverAge { get; set; }

        [Required(ErrorMessage = "Lütfen bir minimum sürücü deneyimi giriniz!")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimum sürücü deneyemi negatif bir değer olamaz!")]
        public int? MinDrivingExperience { get; set; }

        [Required(ErrorMessage = "Lütfen bir Findex puanı giriniz!")]
        [Range(0, 1900, ErrorMessage = "Findex puanı 0 ile 1900 arasında olamalıdır!")]
        public int? MinFindexScore { get; set; }

        [Required(ErrorMessage = "Lütfen bir bagaj boyutu seçiniz!")]
        [Range(1, 3, ErrorMessage = "Lütfen bir bagaj boyutu seçiniz!")]
        public LuggageCapacity? LuggageCapacity { get; set; }

        [Required(ErrorMessage = "Lütfen bir vites tipi seçiniz!")]
        [Range(1, 3, ErrorMessage = "Lütfen bir vites tipi seçiniz!")]
        public TransmissionType? TransmissionType { get; set; }
    }
}
