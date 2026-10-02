namespace RentACar.Core.Utilities.Mailing
{
    /// <summary>
    /// Sistemin dış dünyaya göndereceği e-posta kargo kutusudur (DTO).
    /// </summary>
    public class MailRequest
    {
        public string ToEmail { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
