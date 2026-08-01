using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class CarImageRepository : Repository<CarImage>, ICarImageRepository
    {
        private readonly RentACarContext _context;
        public CarImageRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CarImage>> GetImagesWithCarDetailsAsync(int carId)
        {
            return await _context.CarImages
                .Where(ci => ci.CarId == carId)
                .Include(ci => ci.Car)
                .ThenInclude(ci=>ci.Brand)
                .ToListAsync();
        }
    }
}
