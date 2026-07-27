using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    // IRepository<Car>'dan miras alarak Ekle/Sil/Güncelle yeteneklerini otomatik kazanıyor.
    public interface ICarRepository : IRepository<Car>
    {
        // Bu da bizim bu depocuya verdiğimiz ÖZEL görev:
        Task<List<Car>> GetCarsWithDetailsAsync();

        // YENİ EKLENECEK: Sadece tek bir arabayı (eğer bulamazsa null) detaylarıyla getiren metot
        Task<Car?> GetCarWithDetailsAsync(int id);
    }
}
