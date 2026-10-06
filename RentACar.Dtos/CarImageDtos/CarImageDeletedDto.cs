using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CarImageDtos
{
    public class CarImageDeletedDto : IDto
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = null!;
        public int CarId { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
