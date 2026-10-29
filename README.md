# 🚗 RentACar - Enterprise Level Backend API

Bu proje, kurumsal mimari standartlarına (N-Tier Architecture, Clean Code, SOLID) uygun olarak geliştirilmiş, kapsamlı bir Araç Kiralama (Rent A Car) sisteminin Backend API'sidir. 

Proje, sadece temel CRUD işlemlerini değil; JWT tabanlı güvenliği, dinamik rol yönetimini (Role-Based Access Control), Findex puanı entegrasyonunu, araç bakım (Maintenance) modüllerini ve AOP (Aspect Oriented Programming) yaklaşımlarını barındıran tam donanımlı bir motor olarak tasarlanmıştır.

## 🚀 Proje Vizyonu ve Yol Haritası (Roadmap)
- [x] **Faz 1:** Kurumsal Backend API'nin N-Tier mimariyle inşası ve testlerinin tamamlanması.
- [ ] **Faz 2:** ASP.NET Core MVC kullanılarak projenin Web tarafının MVP (Minimum Viable Product) olarak canlıya alınması. (Şu an geliştiriliyor 🛠️)
- [ ] **Faz 3:** Cross-Platform Mobil Uygulama geliştirme süreçlerinin başlatılması.

## 🛠️ Kullanılan Teknolojiler & Mimari Yaklaşımlar

Projede, modern yazılım endüstrisinin kabul gördüğü en güncel teknolojiler ve "Best Practice" yöntemleri kullanılmıştır:

*   **Platform & Dil:** C#, .NET Core, ASP.NET Core Web API, MVC
*   **Mimari:** N-Tier Architecture (Çok Katmanlı Mimari)
*   **Veri Erişimi (ORM):** Entity Framework Core (Code-First)
*   **Veritabanı:** PostgreSQL (Altyapı MicrosoftSQL Server/MongoDB'ye kolayca entegre edilebilir şekilde soyutlanmıştır.)
*   **IoC Container:** Autofac (Dependency Injection yönetimi için)
*   **Güvenlik:** JWT (JSON Web Token) ile Kimlik Doğrulama ve Dinamik Yetkilendirme
*   **Nesne Eşleme:** AutoMapper (DTO - Entity dönüşümleri için)
*   **Validasyon:** FluentValidation
*   **AOP (Aspect Oriented Programming):** Castle.DynamicProxy (Validation, Caching, Transaction yönetimi için Interceptor yapısı)
*   **Tasarım Prensipleri:** SOLID Prensipleri, DRY, Clean Code

## 🏢 Katmanlı Mimari (N-Tier Architecture) Yapısı

Sistem birbirinden bağımsız, ancak birbiriyle sözleşmeler (Interface'ler) üzerinden konuşan katmanlardan oluşur:

1.  **Core Katmanı:** Projenin kalbidir. Hata yönetimi (Exceptions), Güvenlik (JWT), Interceptor'lar, Evrensel Kurallar ve Base sınıfları barındırır. Başka hiçbir katmana bağımlı değildir.
2.  **Entities Katmanı:** Veritabanı nesnelerimizi (Car, Brand, User, Customer vb.) ve DTO (Data Transfer Object) sınıflarını içerir.
3.  **DataAccess Katmanı:** Veritabanı operasyonlarının (Entity Framework) yönetildiği depomuzdur.
4.  **Business Katmanı:** İş kurallarının (Business Rules) işletildiği mutfaktır. Spagetti koddan kaçınılmış, her kural "BusinessRules Engine" üzerinden modüler olarak kontrol edilmiştir.
5.  **WebAPI Katmanı:** Dış dünyanın (MVC, Mobil Uygulama, React vb.) sistemle konuştuğu gişedir.

## 🤝 Geliştirme Süreci & AI Pair Programming (Eşli Programlama)

Bu proje, modern yazılım endüstrisinin en büyük gücü olan **AI Destekli Pair Programming** vizyonuyla geliştirilmiştir. 

Projedeki kodların tamamı, kas hafızasını güçlendirmek ve mimari disiplini içselleştirmek amacıyla tarafımca **manuel olarak** yazılmıştır. Ancak;
* N-Tier mimarinin tasarlanması,
* SOLID ve Clean Code prensiplerinin projeye entegre edilmesi, 
* Spagetti kodların refactor (temizleme) süreçlerinde, 

**Google Gemini** bir "Senior Mentor ve Kodlama Arkadaşı" olarak aktif danışmanlık yapmış, projenin kurumsal standartlara ulaşmasında omuz omuza bir çalışma yürütülmüştür. 

## 🚀 Kurulum ve Çalıştırma (How to Run)

Projeyi kendi bilgisayarınızda (Local) ayağa kaldırmak için aşağıdaki adımları izleyebilirsiniz:

**1. Gereksinimler (Prerequisites):**
* .NET SDK 
* PostgreSQL (PgAdmin)

**2. Veritabanı Tesisatını Bağlama (Connection String):**
`RentACar.API` klasörünün içindeki `appsettings.json` dosyasını açın ve PostgreSQL bağlantı bilgilerinizi girin:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=5432;Database=RentACarDb;User Id=postgres;Password=sizin_sifreniz;"
}
(Not: E-posta ve Token gibi hassas veriler güvenlik amacıyla secrets.json (User Secrets) üzerinden yönetilmektedir.)

**3. Şantiyeyi İnşa Etme (Migrations):
Visual Studio'da Package Manager Console penceresini açın. Üst kısımdaki Default project menüsünden RentACar.DataAccess katmanını seçerek tabloları oluşturun:

PowerShell
Update-Database
**4. Projeyi Başlatma:RentACar.API projesini başlangıç projesi (Startup Project) olarak ayarlayın ve F5 ile çalıştırın.

## 👨‍💻 Geliştirici
**Buğra Balım** - *Management Information Systems (MIS) Student & Backend Developer*
[GitHub Profilim](https://github.com/bugrabalim11)

---
*Not: Bu proje, sektör standartlarında kod kalitesi ve mimari disiplin elde etmek amacıyla sürekli olarak refactor edilmekte ve geliştirilmektedir.*
