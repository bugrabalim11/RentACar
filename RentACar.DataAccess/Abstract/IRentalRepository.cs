using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<List<Rental>> GetRentalsWithDetailsAsync();
        Task<Rental?> GetRentalWithDetailsByIdAsync(int id);
        Task<List<Rental>> GetRentalsByUserIdAsync(int userId);
    }
}
