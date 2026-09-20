namespace RentACar.MVC.Areas.Admin.Models.RentalDtos
{
    public class RentalResultDto
    {
        // TODO Geri getir butonu ekle (Restore)
        public int Id { get; set; }
        public string ModelName { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Not: Dropdown'da kullanıcı dostu (UX) görünüm için computed property kullanıldı.
        public string CustomerFullName => $"{FirstName} {LastName}";
        public string CarInfo => $"{BrandName} {ModelName}";
    }
}
