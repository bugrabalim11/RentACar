namespace RentACar.Core.Exceptions
{
    public class BusinessException : Exception
    {
        // Constructor (Sınıf doğarken dışarıdan mesajı alır)
        // : base(message) diyerek bu mesajı direkt ana Exception sınıfına fırlatır.
        public BusinessException(string message) : base(message)
        {
            // İçeriye ekstra bir şey yazmana gerek yok, baba sınıf mesajı halledecek!
        }
    
    }
}
