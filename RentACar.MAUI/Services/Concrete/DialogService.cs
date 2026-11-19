using RentACar.MAUI.Services.Abstract;

namespace RentACar.MAUI.Services.Concrete
{
    /// <summary>
    /// MAUI pencere yapısı üzerinden kullanıcıya uyarı mesajları gösteren servis.
    /// </summary>
    public class DialogService : IDialogService
    {
        public async Task ShowAlertAsync(string title, string message, string cancel = "Tamam")
        {
            if (Application.Current?.Windows.Count > 0 && Application.Current.Windows[0].Page != null)
            {
                await Application.Current.Windows[0].Page!.DisplayAlertAsync(title, message, cancel);
            }
        }
    }
}
