namespace RentACar.MAUI.Services.Abstract
{
    /// <summary>
    /// Kullanıcıya bilgi, uyarı ve onay pencereleri göstermek için merkezi arayüz.
    /// </summary>
    public interface IDialogService
    {
        Task ShowAlertAsync(string title, string message, string cancel = "Tamam");
    }
}
