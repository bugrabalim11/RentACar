Branch: feat/maui-car-detail

 - Sıradaki Adım: CarDetailViewModel yapısını kurmak, DashboardView'dan seçili aracın Id'sini detay sayfasına taşımak (Navigation) 
ve CoverImagesUrl listesi için emülatör URL dönüşümünü (10.0.2.2) ayarlamak.

26.11.2026: CarDto → CarResultDto refactor'u yapıldı, CarDetailDto eklendi, ICarService ve CarService içine hata/null korumalı 
GetCarDetailAsync(int carId) metodu yazıldı ve build alındı.