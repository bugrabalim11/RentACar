using RentACar.Core.Utilities.Results;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace RentACar.Core.Utilities.Mailing
{
    public class MailManager : IMailService
    {
        // 1. İnsan Kaynakları Bekçisi (Not kağıdını okuyacak alet)
        private readonly IConfiguration _configuration;

        public MailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IResult> SendEmailAsync(MailRequest mailRequest)
        {
            // 2. Zarfı Oluştur (MimeMessage)
            var emailMessage = new MimeMessage();

            // Kimden gidiyor? (Not kağıdından oku ve zarfa yaz)
            // AppSettings.json dosyasındaki verileri ben doldurdum %100 dolu olduğu için ! koydum.
            string senderName = _configuration.GetSection("EmailConfiguration:SenderName").Value!;
            string senderEmail = _configuration.GetSection("EmailConfiguration:SenderEmail").Value!;

            // Gönderici adresini oluşturup zarfın (emailMessage) "Kimden" (From) kısmına mühürlüyoruz.
            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));

            // Kime gidiyor? (Parametre olarak gelen kutudan al ve zarfa yaz)
            emailMessage.To.Add(new MailboxAddress("", mailRequest.ToEmail));

            // Konu ne?
            emailMessage.Subject = mailRequest.Subject;

            // 3. Mektubu Yaz (BodyBuilder)
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailRequest.Body; // HTML formatında göndereceğiz
            emailMessage.Body = bodyBuilder.ToMessageBody(); // Mektubu zarfın içine koyduk.

            // 4. Kamyonu Çalıştır ve Yola Çık (SmtpClient)
            // 'using' kullanıyoruz ki mail atıldıktan sonra kamyon RAM'de yer kaplamasın, anında yok edilsin.
            using (var client = new SmtpClient())
            {
                try
                {
                    // Sunucuya Bağlan (Port 587 ve TLS şifrelemesi kullanıyoruz)
                    // AppSettings.json dosyasındaki verileri ben doldurdum %100 dolu olduğu için ! koydum.
                    string smtpServer = _configuration.GetSection("EmailConfiguration:SmtpServer").Value!;
                    int smtpPort = Convert.ToInt32(_configuration.GetSection("EmailConfiguration:SmtpPort").Value!);

                    await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                    // Güvenliğe Kimlik Göster (Login ol)
                    string password = _configuration.GetSection("EmailConfiguration:Password").Value!;
                    await client.AuthenticateAsync(senderEmail, password);

                    // Kargoyu Teslim Et
                    await client.SendAsync(emailMessage);

                    // Dükkanı Kapat
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    // Kamyon yolda kaza yaparsa veya şifre yanlışsa buraya düşer
                    return new ErrorResult($"Email gönderilirken bir hata oluştu: {ex.Message}");
                }
            }

            // Kargo başarıyla teslim edildi fişi dön
            return new SuccessResult("Fatura e-postası başarıyla gönderildi.");
        }
    }
}
