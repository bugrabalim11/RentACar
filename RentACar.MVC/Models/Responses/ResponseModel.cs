namespace RentACar.MVC.Models.Responses
{
    // SENIOR NOTU: Bu Jenerik (Generic <T>) sınıf, projemizdeki DTO çöplüğünü (Class Explosion) bitirmek için tasarlandı.
    // Artık her sayfa için (Araba, Marka, Bakım vb.) ayrı ayrı ResponseDto yazmayacağız.
    // API'den gelen standart kargo kolisinin dışı her zaman aynıdır (Success, Message). 
    // İçindeki asıl ürün ise (T Data) isteğe göre şekil alır. DRY (Kendini Tekrar Etme) kuralının zirvesidir.
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;

        // T bir Referans Tipi (class) mi yoksa Değer Tipi (int, bool) mi henüz bilmediğimiz için 'null' kullanamayız.
        // Bunun yerine 'default!' (T'nin fabrika ayarı neyse o olsun ve bana güven) diyoruz.
        public T Data { get; set; } = default!;
    }
}
