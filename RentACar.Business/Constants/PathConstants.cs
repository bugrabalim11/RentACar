namespace RentACar.Business.Constants
{
    // SENİOR NOTU: Projedeki tüm dosya yolları (Magic Strings) burada toplanır.
    // Bu sınıf, şantiyenin yön tabelasıdır. Yarın resimlerin kaydedileceği klasör ismi 
    // değişirse (örn: "wwwroot/Uploads/Cars" olursa), projede 50 yeri aramak yerine 
    // sadece buradaki tabelayı güncelleriz.
    public static class PathConstants
    {
        // Linux ve Windows uyumluluğu için ters bölü (\) değil, düz bölü (/) kullanıyoruz.
        public static string ImagesPath = "wwwroot/Images"; 
        public static string DefaultImagePath = "wwwroot/Images/default.jpg";
    }
}