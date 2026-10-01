using RentACar.Core.Utilities.Results;

namespace RentACar.Core.Utilities.Mailing
{
    public class MailManager : IMailService
    {
        public Task<IResult> SendEmailAsync(MailRequest mailRequest)
        {
            throw new NotImplementedException();
        }
    }
}
