using RentACar.Core.Utilities.Mailing;

namespace RentACar.Business.Helpers
{
    public static class MailTemplateHelper
    {
        // Kiralama işlemi başarılı olduğunda basılacak fatura şablonu
        public static MailRequest CreateRentalSuccessMail(string customerName, string customerEmail, string carBrand, decimal totalAmount, DateTime rentDate, DateTime? returnDate)
        {
            string htmlBody = $@"
                <h2>Sayın {customerName}, BUM! Kiralama Başarılı!</h2>
                <p>Seçtiğiniz <b>{carBrand}</b> marka aracın kiralama işlemi tamamlanmıştır.</p>
                <p>Ödenen tutar: <b>{totalAmount} TL</b></p>
                <p>Kiralama tarihi: <b>{rentDate:dd.MM.yyyy}</b></p>
                <p>İade tarihi: <b>{returnDate?.ToString("dd.MM.yyyy") ?? "Belirtilmemiş"}</b></p>
                <p>Bizi tercih ettiğiniz için teşekkür ederiz.</b></p>";

            return new MailRequest
            {
                ToEmail = customerEmail,
                Subject = "Rent A Car - Kiralama Faturanız",
                Body = htmlBody
            };
        }
    }
}
