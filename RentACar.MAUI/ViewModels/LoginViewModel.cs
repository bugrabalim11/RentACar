using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using System.Windows.Input;

namespace RentACar.MAUI.ViewModels
{
    /// <summary>
    /// Giriş ekranının veri bağlamalarını ve kimlik doğrulama akışını yönetir.
    /// </summary>
    public class LoginViewModel
    {
        private readonly IAuthService _authService;
        private readonly IDialogService _dialogService;

        public LoginViewModel(IAuthService authService, IDialogService dialogService)
        {
            _authService = authService;
            _dialogService = dialogService;
            LoginCommand = new Command(async () => await PerformLoginAsync());
        }

        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public ICommand LoginCommand { get; }

        /// <summary>
        /// Kullanıcı bilgilerini doğrulayıp API üzerinden giriş yapar ve dönen token'ı saklar.
        /// </summary>
        private async Task PerformLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await _dialogService.ShowAlertAsync("Uyarı", "Lütfen e-posta ve şifre alanlarını doldurunuz!");
                return;
            }

            var requestDto = new LoginRequestDto
            {
                Email = this.Email,
                Password = this.Password
            };

            try
            {
                var responseDto = await _authService.LoginAsync(requestDto);
                if (responseDto != null)
                {
                    await SecureStorage.Default.SetAsync("AuthToken", responseDto.Token);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (Application.Current?.Windows.Count > 0)
                        {
                            Application.Current?.Windows[0].Page = new AppShell();
                        }
                    });
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Hata", "E-posta veya şifre hatalı!", "Tekrar Dene");
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Bağlantı Hatası", $"Sunucuya ulaşılamadı. Detay:{ex.Message}");
            }
        }
    }
}