using Microsoft.AspNetCore.Http;

namespace RentACar.Core.Utilities.Helpers.FileHelper
{
    public class FileHelperManager : IFileHelper
    {
        public void Delete(string filePath)
        {
            // Veritabanından gelen "Images/guid.jpg" yolunu fiziksel depo yoluna çeviriyoruz
            string physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath);

            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
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

                // 3. FİZİKSEL DEPO ADRESİNİ BUL (C:\...\wwwroot\Images)
                // (Masaüstü mü, Linux sunucu mu?
                string currentDirectory = Directory.GetCurrentDirectory();
                string physicalPath = Path.Combine(currentDirectory, "wwwroot", root);

                // 4. KLASÖR YOKSA AÇ
                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                // 5. DOSYAYI FİZİKSEL OLARAK KAYDET
                string fullPhysicalFilePath = Path.Combine(physicalPath, newFileName);
                using (FileStream fileStream = File.Create(fullPhysicalFilePath))
                {
                    // Kargo bandı çalıştı!
                    file.CopyTo(fileStream);
                }

                // 6. VERİTABANI İÇİN TERTEMİZ "GÖRECELİ" YOLU DÖN (Images/guid.jpg)
                // Path.Combine(root, newFileName) -> "Images\guid.jpg" yapar.
                // Replace ile Windows'un ters slash'ini, İnternetin düz slash'ine (/) çeviriyoruz!
                return Path.Combine(root, newFileName).Replace("\\","/");
            }
            return null;
        }
    }
}
