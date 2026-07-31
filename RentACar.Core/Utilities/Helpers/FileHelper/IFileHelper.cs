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

        /// <summary>
        /// Belirtilen yoldaki dosyayı sunucudan siler
        /// </summary>
        /// <param name="filePath"></param>
        void Delete(string filePath);

        /// <summary>
        /// file = yeni koli, filePath = silinecek eski adres, root = yeni kolinin konacağı depo
        /// </summary>
        /// <param name="file"></param>
        /// <param name="filePath"></param>
        /// <param name="root"></param>
        /// <returns></returns>
        string? Update(IFormFile file, string filePath, string root);
    }
}
