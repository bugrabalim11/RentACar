using Microsoft.AspNetCore.Http;
using RentACar.Core.Entities.Signatures;

namespace RentACar.Dtos.CarImageDtos
{
    public class CarImageUpdateDto : IDto
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}
