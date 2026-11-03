using RentACar.Core.Utilities.Mailing;

namespace RentACar.Business.Helpers
{
    /// <summary>
    /// E-posta gönderimleri için HTML formatında dinamik mail şablonları oluşturur.
    /// </summary>
    public static class MailTemplateHelper
    {
        public static MailRequest CreateRentalSuccessMail(string customerFullName, string customerEmail, string carFullName, decimal totalAmount, DateTime rentDate, DateTime? returnDate)
        {
            string htmlBody = $@"
                <h2>Sayın {customerFullName}, Kiralama İşleminiz Tamamlandı.</h2>
                <p>Seçtiğiniz <b>{carFullName}</b> marka aracın kiralama işlemi tamamlanmıştır.</p>
                <p>Ödenen tutar: <b>{totalAmount} TL</b></p>
                <p>Kiralama tarihi: <b>{rentDate:dd.MM.yyyy}</b></p>
                <p>İade tarihi: <b>{returnDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}</b></p>
                <p><b>Bizi tercih ettiğiniz için teşekkür ederiz. İyi yolculuklar dileriz.</b></p>";

            return new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "Rent A Car - Kiralama Faturanız",
                Body = htmlBody
            };
        }

        public static MailRequest CreateRentalUpdateMail(string customerFullName, string customerEmail, string carFullName, decimal totalAmount, decimal? differenceAmount, DateTime rentDate, DateTime? returnDate)
        {
            string htmlBody = $@"
                <h2>Sayın {customerFullName}, Kiralama Güncelleme İşleminiz Tamamlandı.</h2>
                <p>Seçtiğiniz <b>{carFullName}</b> marka aracın kiralama işlemi güncellenmiştir.</p>
                <p>Ödenen toplam tutar: <b>{totalAmount} TL</b></p>
                <p>Ödenen fark tutar: <b>{differenceAmount ?? 0} TL</b></p>
                <p>Kiralama tarihi: <b>{rentDate:dd.MM.yyyy}</b></p>
                <p>İade tarihi: <b>{returnDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}</b></p>
                <p><b>Bizi tercih ettiğiniz için teşekkür ederiz. İyi yolculuklar dileriz.</b></p>";

            return new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "Rent A Car - Kiralama Güncelleme Faturanız",
                Body = htmlBody
            };
        }
    }
}
