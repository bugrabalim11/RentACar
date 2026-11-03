using Microsoft.AspNetCore.Http;

namespace RentACar.Core.Utilities.Helpers.FileHelper
{
    public class FileHelperManager : IFileHelper
    {
        public void Delete(string filePath)
        {
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
                string extension = Path.GetExtension(file.FileName);
                string newFileName = Guid.NewGuid().ToString() + extension;

                string currentDirectory = Directory.GetCurrentDirectory();
                string physicalPath = Path.Combine(currentDirectory, "wwwroot", root);

                if (!Directory.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }

                string fullPhysicalFilePath = Path.Combine(physicalPath, newFileName);

                using (FileStream fileStream = File.Create(fullPhysicalFilePath))
                {
                    file.CopyTo(fileStream);
                }

                return Path.Combine(root, newFileName).Replace("\\","/");
            }
            return null;
        }
    }
}
