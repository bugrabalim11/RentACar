using Microsoft.AspNetCore.Http;
using RentACar.Core.Entities;

namespace RentACar.Dtos.CarImageDtos
{
    public class CarImageAddDto : IDto
    {
        public int CarId { get; set; }
        public IFormFile ImageFile { get; set; } = null!;
    }
}
