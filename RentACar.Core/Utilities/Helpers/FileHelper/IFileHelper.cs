using Microsoft.AspNetCore.Http;

namespace RentACar.Core.Utilities.Helpers.FileHelper
{
    /// <summary>
    /// Proje genelinde dosya yükleme, silme ve güncelleme işlemlerini yöneten sözleşmedir.
    /// </summary>
    public interface IFileHelper
    {
        string? Upload(IFormFile file, string root);
        void Delete(string filePath);
        string? Update(IFormFile file, string filePath, string root);
    }
}
