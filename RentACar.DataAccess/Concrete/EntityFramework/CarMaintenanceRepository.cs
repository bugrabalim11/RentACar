using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class CarMaintenanceRepository : Repository<CarMaintenance>, ICarMaintenanceRepository
    {
        private readonly RentACarContext _context;
        public CarMaintenanceRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CarMaintenance?> GetCarMaintenanceByIdWithDetailsAsync(int id)
        {
            return await _context.CarMaintenances
                .AsNoTracking()
                .Include(c => c.Car).ThenInclude(c => c.Brand)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CarMaintenance>> GetCarMaintenanceWithDetailsAsync()
        {
            return await _context.CarMaintenances
                // Sadece listeleme yapıyoruz, veriyi değiştirmeyeceğiz. Dedektiflere gerek yok! Performansı uçururur
                .AsNoTracking()
                .Include(c => c.Car).ThenInclude(c => c.Brand)
                .ToListAsync();
        }
    }
}
