using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface ICarImageRepository : IRepository<CarImage>
    {
        Task<List<CarImage>> GetImagesWithCarDetailsAsync(int carId);
        /// <summary>
        /// EF Core'un SaveChanges (Soft Delete) mekanizmasına takılmadan, doğrudan veritabanında fiziksel silme (Hard Delete) işlemi yapar.
        /// </summary>
        /// <param name="id">Silinecek resmin benzersiz kimliği.</param>
        /// <returns></returns>
        Task HardDeleteByIdAsync(int id);
    }
}
