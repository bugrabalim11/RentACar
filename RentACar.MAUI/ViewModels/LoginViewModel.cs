using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using System.Windows.Input;

namespace RentACar.MAUI.ViewModels
{
    /// <summary>
    /// Uygulamanın giriş ekranındaki kullanıcı etkileşimlerini, 
    /// veri bağlamalarını ve kimlik doprulama opeerasyonlarını yöneten ViewMoedl sınıfıdır.
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
        /// Kullanıcının girdiği e-posta ve şifre bilgilerini DTO'ya paketleyip, AuythService üzerinden API'a göneririr.
        /// Başarılı olursa döenen token'ı işler, başarısız olursa kullanıcıya uyarı verir
        /// </summary>
        private async Task PerformLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrEmpty(Password))
            {
                // TODO: İleride buraya ekrana fırlayan bir "Lütfen alanları doldurun" uyarı kutusu (DisplayAlert) ekleyeceğiz.
                return;
            }

            var requestDto = new LoginRequestDto
            {
                Email = this.Email,
                Password = this.Password
            };

            var responseDto = await _authService.LoginAsync(requestDto);

            if (responseDto != null)
            {
                // TODO: İleride bu Token'ı donanımsal çelik kasaya (SecureStorage) kilitleyip, Ana Sayfaya yönlendirme yapacağız.
                Console.WriteLine($"Giriş başarılı! Gelen Token: {responseDto.Token}");
            }
            else
            {
                // TODO: İleride buraya "E-posta veya şifre hatalı" uyarısı ekleyeceğiz.
                Console.WriteLine($"Giriş başarısız! Kurye eli boş döndü.");
            }
        }
    }
}
