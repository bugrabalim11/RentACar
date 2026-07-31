using Microsoft.AspNetCore.Http;

namespace RentACar.Core.Utilities.Helpers.FileHelper
{
    public interface IFileHelper
    {
        /// <summary>
        /// Dışarıdan gelen dosyayı sunucuda belirtilen klasöre kaydeder ve yeni dosya yolunu döndürür.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="root">Kaydedilecek klasörün fiziki yolu</param>
        /// <returns></returns>
        string? Upload(IFormFile file, string root);
    }
}
