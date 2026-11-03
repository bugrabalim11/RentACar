using RentACar.Core.Utilities.Results;

namespace RentACar.Core.Utilities.Mailing
{
    /// <summary>
    /// Proje genelinde e-posta gönderim operasyonlarını soyutlayan temel arayüzdür.
    /// </summary>
    public interface IMailService
    {
        Task<IResult> SendEmailAsync(MailRequest mailRequest);
    }
}
