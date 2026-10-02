using RentACar.Core.Utilities.Mailing;

namespace RentACar.Business.Helpers
{
    public static class MailTemplateHelper
    {
        // Kiralama işlemi başarılı olduğunda basılacak fatura şablonu
        public static MailRequest CreateRentalSuccessMail(string customerFullName, string customerEmail, string carFullName, decimal totalAmount, DateTime rentDate, DateTime? returnDate)
        {
            string htmlBody = $@"
                <h2>Sayın {customerFullName}, BUM! Kiralama Başarılı!</h2>
                <p>Seçtiğiniz <b>{carFullName}</b> marka aracın kiralama işlemi tamamlanmıştır.</p>
                <p>Ödenen tutar: <b>{totalAmount} TL</b></p>
                <p>Kiralama tarihi: <b>{rentDate:dd.MM.yyyy}</b></p>
                <p>İade tarihi: <b>{returnDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}</b></p>
                <p><b>Bizi tercih ettiğiniz için teşekkür ederiz.</b></p>";

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
                <h2>Sayın {customerFullName}, BUM! Kiralama Güncellemesi Başarılı!</h2>
                <p>Seçtiğiniz <b>{carFullName}</b> marka aracın kiralama işlemi güncellenmiştir.</p>
                <p>Ödenen toplam tutar: <b>{totalAmount} TL</b></p>
                <p>Ödenen fark tutar: <b>{differenceAmount ?? 0} TL</b></p>
                <p>Kiralama tarihi: <b>{rentDate:dd.MM.yyyy}</b></p>
                <p>İade tarihi: <b>{returnDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}</b></p>
                <p><b>Bizi tercih ettiğiniz için teşekkür ederiz.</b></p>";

            return new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "Rent A Car - Kiralama Güncelleme Faturanız",
                Body = htmlBody
            };
        }
    }
}
