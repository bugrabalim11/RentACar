using RentACar.Core.Entities;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalListDto : IDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
