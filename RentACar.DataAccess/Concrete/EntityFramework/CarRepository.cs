using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class CarRepository : Repository<Car>, ICarRepository
    {
        private readonly RentACarContext _context;

        public CarRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetCarsWithDetailsAsync()
        {
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Color)
                .Include(c => c.CarImages)
                .ToListAsync();
        }

        public async Task<Car?> GetCarWithDetailsAsync(int id)
        {
            return await _context.Cars
                .Include(c => c.Brand)
                .Include(c => c.Color)
                .Include(c => c.CarImages)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
