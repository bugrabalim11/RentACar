using RentACar.Core.Entities;

namespace RentACar.Entities.Concrete
{
    public class CarImage : IEntity
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public string ImagePath { get; set; } = null!;
        public DateTime UploadDate { get; set; }

        public Car Car { get; set; } = null!;
    }
}
