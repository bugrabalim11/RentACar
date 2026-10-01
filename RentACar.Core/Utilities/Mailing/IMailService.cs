using RentACar.Core.Utilities.Results;

namespace RentACar.Core.Utilities.Mailing
{
    /// <summary>
    /// Projedeki e-posta gönderim işlemlerinin evrensel sözleşmesidir.
    /// Yarın MailKit yerine başka bir sisteme geçilirse, sadece bu sözleşmeyi uygulayan yeni bir sınıf yazılır.
    /// </summary>
    public interface IMailService
    {
        /// <summary>
        /// Verilen MailRequest (Zarf) içindeki bilgileri kullanarak asenkron e-posta gönderir.
        /// </summary>
        /// <param name="mailRequest">Gönderilecek e-postanın detayları (Kime, Konu, İçerik)</param>
        /// <returns>İşlemin başarılı olup olmadığını dönen standart IResult.</returns>
        Task<IResult> SendEmailAsync(MailRequest mailRequest);
    }
}
