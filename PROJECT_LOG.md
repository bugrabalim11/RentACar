- Sıradaki Adım: Araç kiralama (Rental) akışını kurmak; CarDetailView'a "Kirala" butonu ekleyip tarih seçimi ve 
API kiralama isteği (POST) sayfasını geliştirmek.

Branch: main (feat/maui-car-detail tamamlandi)

 28.11.2026: CarDetailViewModel (INotifyPropertyChanged, QueryProperty ve 10.0.2.2 resim URL dönüşümü) yazıldı. 
 DashboardView'dan CarDetailView'a navigasyon bağlandı (x:Reference ile geri dönüşteki NullReferenceException çözüldü). 
 CarDetailView için CarouselView fotoğraf galerisi, teknik özellikler (Grid) ve kiralama şartları arayüzü tamamlandı.

 26.11.2026: CarDto → CarResultDto refactor'u yapıldı, CarDetailDto eklendi, ICarService ve CarService içine hata/null korumalı 
 GetCarDetailAsync(int carId) metodu yazıldı ve build alındı.