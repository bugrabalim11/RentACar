using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface ICarImageRepository : IRepository<CarImage>
    {
        Task<List<CarImage>> GetImagesWithCarDetailsAsync(int carId);
    }
}
