using RentACar.Core.Entities;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalListDto : IDto
    {
        public int Id { get; set; }
        public string CarModelName { get; set; } = null!;

        // Car + Brand + Name. Özelliğin adını CarBrandName yaparsan, AutoMapper anında
        // "Tamam, Araba'nın içindeki Marka'nın Name'ini alacağım" diyecektir.
        public string CarBrandName { get; set; } = null!;
        public string CustomerFirstName { get; set; } = null!;
        public string CustomerLastName { get; set; } = null!;
        public string PickUpOfficeName { get; set; } = null!;
        public string DropOffOfficeName { get; set; } = null!;
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
