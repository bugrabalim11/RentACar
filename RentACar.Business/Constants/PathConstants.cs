namespace RentACar.Business.Constants
{
    // SENİOR NOTU: Projedeki tüm dosya yolları (Magic Strings) burada toplanır.
    // Bu sınıf, şantiyenin yön tabelasıdır. Yarın resimlerin kaydedileceği klasör ismi 
    // değişirse (örn: "wwwroot/Uploads/Cars" olursa), projede 50 yeri aramak yerine 
    // sadece buradaki tabelayı güncelleriz.
    public static class PathConstants
    {
        // Artık wwwroot yok! Sadece klasör adı.
        public static string ImagesPath = "Images"; 
        public static string DefaultImagePath = "Images/default.jpg";
    }
}