using Microsoft.Extensions.Logging;
using RentACar.MAUI.Services.Abstract;
using RentACar.MAUI.Services.Concrete;

namespace RentACar.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            // --- DEPENDENCY INJECTION & HTTP CLIENT REGISTRATION ---
            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                // DİKKAT: Android emülatörü "localhost" anlamaz, PC'ye ulaşmak için 10.0.2.2 kullanır!
                client.BaseAddress = new Uri("http://10.0.2.2:7085/");
            });

            return builder.Build();
        }
    }
}
