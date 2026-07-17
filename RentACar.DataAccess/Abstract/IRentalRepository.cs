using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Abstract
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<List<Rental>> GetRentalsWithDetailsAsync();
        Task<Rental?> GetRentalWithDetailsByIdAsync(int id);
    }
}
