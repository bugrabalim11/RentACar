using RentACar.Core.Entities;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalDetailDto : IDto
    {
        public int Id { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string BrandName { get; set; } = null!;
        public string ModelName { get; set; } = null!;
        public decimal DailyPrice { get; set; }
        public string Plate { get; set; } = null!;
        public int MinDrivingExperience { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PickUpOfficeName { get; set; } = null!;
        public string DropOffOfficeName { get; set; } = null!;
    }
}
