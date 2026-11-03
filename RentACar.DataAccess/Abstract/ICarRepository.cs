using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface ICarRepository : IRepository<Car>
    {
        Task<List<Car>> GetCarsWithDetailsAsync();
        Task<Car?> GetCarWithDetailsAsync(int id);
    }
}
