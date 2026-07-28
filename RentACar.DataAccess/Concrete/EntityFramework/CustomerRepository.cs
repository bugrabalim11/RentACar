using Microsoft.EntityFrameworkCore;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly RentACarContext _context;
        public CustomerRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersWithDetailsAsync()
        {
            return await _context.Customers
                .Include(c => c.User)
                .ToListAsync();

        }

        public async Task<Customer?> GetCustomerWithDetailsAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.User)
                .Include(c=>c.Rentals)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
