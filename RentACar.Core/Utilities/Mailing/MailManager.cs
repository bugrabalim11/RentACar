using RentACar.Core.Utilities.Results;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace RentACar.Core.Utilities.Mailing
{
    /// <summary>
    /// MailKit kütüphanesini kullanarak SMTP protokolü üzerinden e-posta gönderim işlemlerini gerçekleştirir.
    /// </summary>
    public class MailManager : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IResult> SendEmailAsync(MailRequest mailRequest)
        {
            var emailMessage = new MimeMessage();

            string senderName = _configuration.GetSection("EmailConfiguration:SenderName").Value!;
            string senderEmail = _configuration.GetSection("EmailConfiguration:SenderEmail").Value!;

            emailMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            emailMessage.To.Add(new MailboxAddress("", mailRequest.ToEmail));
            emailMessage.Subject = mailRequest.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailRequest.Body;  
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    string smtpServer = _configuration.GetSection("EmailConfiguration:SmtpServer").Value!;
                    int smtpPort = Convert.ToInt32(_configuration.GetSection("EmailConfiguration:SmtpPort").Value!);

                    await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);

                    string password = _configuration.GetSection("EmailConfiguration:Password").Value!;
                    await client.AuthenticateAsync(senderEmail, password);

                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    return new ErrorResult($"Email gönderilirken bir hata oluştu: {ex.Message}");
                }
            }
            return new SuccessResult("Fatura e-postası başarıyla gönderildi.");
        }
    }
}
