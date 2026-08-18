using RentACar.Core.Entities;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalUpdateReturnDateDto : IDto
    {
        public DateTime ReturnDate { get; set; }
    }
}
