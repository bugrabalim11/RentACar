using RentACar.Business.Abstract;

namespace RentACar.API.BackgroundServices
{
    /// <summary>
    /// Her 24 saatte bir veritabanını tarayarak, kullanılmayan veya eski araç görsellerini sistemden temizleyen arka plan servisidir.
    /// </summary>
    public class CarImageCleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CarImageCleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var carImageService = scope.ServiceProvider.GetRequiredService<IAdminCarImageService>();

                    try
                    {
                        await carImageService.DeleteOldImagesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Temizlik sırasında hata: {ex.Message}");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
