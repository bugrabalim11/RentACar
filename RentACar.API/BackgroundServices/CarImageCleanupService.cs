using RentACar.Business.Abstract;

namespace RentACar.API.BackgroundServices
{
    // BackgroundService'den miras alarak bu sınıfı "Ölümsüz bir Gece Bekçisine" çeviriyoruz.
    public class CarImageCleanupService : BackgroundService
    {
        // Bekçimiz ölümsüz (Singleton) olduğu için ölümlü (Scoped) aşçılarla doğrudan çalışamaz.
        // Bu yüzden ona aşçıları üretecek bir 'Vardiya Fabrikası' (Scope Factory) veriyoruz.
        private readonly IServiceScopeFactory _scopeFactory;

        public CarImageCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        // ExecuteAsync: Bekçinin devriye gezdiği ana koridordur. Proje çalıştığı an burası tetiklenir.
        // stoppingToken: Şalteri indirme (iptal) telsizidir.
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // DİKKAT: Başında '!' var. Yani: "İptal sinyali GELMEDİĞİ sürece durmadan dön!"
            while (!stoppingToken.IsCancellationRequested)
            {
                // Fabrikayı çalıştırıp 1 gecelik, işi bitince hafızadan silinecek bir vardiya (Scope) yaratıyoruz.
                using (var scope = _scopeFactory.CreateScope())
                {
                    var carImageService = scope.ServiceProvider.GetRequiredService<ICarImageService>();

                    try
                    {
                        await carImageService.DeleteOldImagesAsync();
                    }
                    catch (Exception)
                    {
                        // boş kalsın
                    }

                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
            }
        }
    }
}
