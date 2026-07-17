using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        private readonly RentACarContext _context;
        public RentalRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Rental>> GetRentalsWithDetailsAsync()
        {
            return await _context.Rentals
               // Sadece listeleme yapıyoruz, veriyi değiştirmeyeceğiz. Dedektiflere gerek yok! Performansı uçururur
               .AsNoTracking()
               .Include(x => x.Car).ThenInclude(c => c.Brand)
               .Include(x => x.Customer)
               .Include(x => x.PickUpOffice)
               .Include(x => x.DropOffOffice)
               .ToListAsync();
        }

        public async Task<Rental?> GetRentalWithDetailsByIdAsync(int id)
        {
            return await _context.Rentals
               .AsNoTracking()
               .Include(x => x.Car).ThenInclude(c => c.Brand)
               .Include(x => x.Customer)
               .Include(x => x.PickUpOffice)
               .Include(x => x.DropOffOffice)
               .FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
