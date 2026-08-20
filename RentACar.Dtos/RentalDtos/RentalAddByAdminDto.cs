using RentACar.Core.Entities;
using RentACar.Dtos.CreditCardInformationDtos;

namespace RentACar.Dtos.RentalDtos
{
    public class RentalAddByAdminDto : IDto
    {
        public int CustomerId { get; set; }
        public int CarId { get; set; }
        public int PickUpOfficeId { get; set; }
        public int DropOffOfficeId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// Böylece müşteri bize istek atarken "Al bu kiralama bilgilerim,
        /// bu da kredi kartım" diyerek ikisini tek pakette yollayacak
        /// </summary>
        public CreditCardInformationDto CreditCardInformation { get; set; } = null!;
    }
}
