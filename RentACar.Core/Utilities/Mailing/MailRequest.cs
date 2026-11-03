namespace RentACar.Core.Utilities.Mailing
{
    /// <summary>
    /// Gönderilecek e-postanın alıcı, konu ve içerik bilgilerini taşıyan veri transfer nesnesidir (DTO).
    /// </summary>
    public class MailRequest
    {
        public string ToEmail { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
