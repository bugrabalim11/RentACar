using Microsoft.AspNetCore.Http;

namespace RentACar.Core.Utilities.Helpers.FileHelper
{
    public class FileHelperManager : IFileHelper
    {
        public void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public string? Update(IFormFile file, string filePath, string root)
        {
            Delete(filePath);

            return Upload(file, root);
        }

        public string? Upload(IFormFile file, string root)
        {
            if (file.Length > 0)
            {
                // 1. Dosya uzantısını alıyoruz (Örn: .jpg, .png)
                string extension = Path.GetExtension(file.FileName);

                // 2. Aynı isimde dosyalar çakışmasın diye benzersiz bir isim (GUID) üretiyoruz
                string newFileName = Guid.NewGuid().ToString() + extension;

                // 3. Eğer belirttiğimiz klasör yoksa, o klasörü oluşturuyoruz.
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                // 4. Klasör yolu ile yeni dosya adını birleştirip tam adresi çıkarıyoruz.
                string imagePath = Path.Combine(root, newFileName);

                using (FileStream fileStream = File.Create(imagePath))
                {
                    // Kargo bandı çalıştı!
                    file.CopyTo(fileStream);
                }

                // SENIOR NOTU: Sunucu (Windows) dosya yollarında ters slash (\) kullanır, ancak İnternet dünyası (URL) düz slash (/) kullanır.
                // Ayrıca veritabanının 'wwwroot' gibi fiziksel sunucu klasörlerini bilmesine gerek yoktur.
                // Bu yüzden veriyi mühürlemeden önce hem 'wwwroot\' kelimesini kesip atıyoruz, hem de web uyumlu olması için slash yönlerini değiştiriyoruz.
                return imagePath.Replace("wwwroot\\", "").Replace("\\", "/");
            }
            return null;
        }
    }
}
