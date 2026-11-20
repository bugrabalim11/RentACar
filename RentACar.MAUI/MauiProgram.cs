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
                // Android emülatörü yerel makineye 10.0.2.2 üzerinden erişir.
                client.BaseAddress = new Uri("https://10.0.2.2:7085/");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
#if DEBUG
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
#endif
                return handler;
            });

            builder.Services.AddTransient<ViewModels.LoginViewModel>();
            builder.Services.AddTransient<Views.LoginView>();
            builder.Services.AddTransient<ViewModels.DashboardViewModel>();
            builder.Services.AddTransient<Views.DashboardView>();

            builder.Services.AddSingleton<IDialogService, DialogService>();

            return builder.Build();
        }
    }
}
