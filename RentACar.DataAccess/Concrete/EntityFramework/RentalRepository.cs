using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        private readonly RentACarContext _context;
        public RentalRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetRentalsByUserIdAsync(int userId)
        {
            return await _context.Rentals
              .AsNoTracking()
              .Include(x => x.Car).ThenInclude(c => c.Brand)
              .Include(x => x.Customer).ThenInclude(c => c.User)
              .Include(x => x.PickUpOffice)
              .Include(x => x.DropOffOffice)
              .Where(x => x.Customer.UserId == userId)
              .ToListAsync();
        }

        public async Task<List<Rental>> GetRentalsWithDetailsAsync()
        {
            return await _context.Rentals
               .AsNoTracking()
               .Include(x => x.Car).ThenInclude(c => c.Brand)
               .Include(x => x.Customer).ThenInclude(c => c.User)
               .Include(x => x.PickUpOffice)
               .Include(x => x.DropOffOffice)
               .ToListAsync();
        }

        public async Task<Rental?> GetRentalWithDetailsByIdAsync(int id)
        {
            return await _context.Rentals
               .AsNoTracking()
               .Include(x => x.Car).ThenInclude(c => c.Brand)
               .Include(x => x.Customer).ThenInclude(c => c.User)
               .Include(x => x.PickUpOffice)
               .Include(x => x.DropOffOffice)
               .FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
