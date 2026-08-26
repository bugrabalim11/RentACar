namespace RentACar.Business.Abstract
{
    /// <summary>
    /// Bu servis, gerçek bir Kredi Kayıt Bürosu (Findeks) entegrasyonu yapılana kadar
    /// müşteriler için 0 ile 1900 arasında rastgele (Mock) finansal puan üreten dublör servistir.
    /// </summary>
    public interface IFindexScoreService
    {
        int GetScoreByCustomerId(int customerId);
    }
}
