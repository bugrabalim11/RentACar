namespace RentACar.Core.Exceptions
{
    /// <summary>
    /// API genelinde fırlatılan tüm hataların, 
    /// MVC/İstemci tarafına tek tipte (standart) gönderilmesini sağlayan
    /// hata şablonu (Acil Durum Zarfı).
    /// </summary>
    public class ErrorDetails
    {
        // Hata mesajı her zaman gelmek zorundadır o yüzden null!;
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }

        // Eğer sistemde bir veritabanı çökmesi (500) veya Business hatası ("Bu araba zaten kiralanmış") yaşanırsa,
        // ortada bir doğrulama (Validation) hatası YOKTUR y üzden boş gelebilir (?).
        public IEnumerable<string>? ValidationErrors { get; set; } 
    }
}
