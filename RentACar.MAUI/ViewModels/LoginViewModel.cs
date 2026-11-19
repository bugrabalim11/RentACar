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

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
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
            // TODO: UI bağımlılığını kaldırmak için IDialogService eklenecek.
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                if (Application.Current?.Windows.Count > 0)
                {
                    await Application.Current.Windows[0].Page!.DisplayAlertAsync("Uyarı", "Lütfen e-posta ve şifre alanlarını doldurunuz.", "Tamam");
                }
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
                    // TODO: Giriş sonrası ana sayfaya (Dashboard/AppShell) yönlendirilecek.
                }
                else
                {
                    if (Application.Current?.Windows.Count > 0)
                    {
                        await Application.Current.Windows[0].Page!.DisplayAlertAsync("Hata", "E-posta veya şifre hatalı!", "Tekrar Dene");
                    }
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.Windows.Count > 0)
                {
                    await Application.Current.Windows[0].Page!.DisplayAlertAsync("Bağlantı Hatası", $"Sunucuya ulaşılamadı. Detay: {ex.Message}", "Tamam");
                }
            }
        }
    }
}