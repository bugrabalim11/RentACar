using RentACar.Core.Entities;

namespace RentACar.Dtos.CarImageDtos
{
    public class CarImageDetailDto : IDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string CarName { get; set; } = null!;
        public string ImagePath { get; set; } = null!;
        public DateTime UploadDate { get; set; }
    }
}
