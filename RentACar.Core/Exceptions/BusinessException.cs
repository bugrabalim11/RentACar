namespace RentACar.Core.Exceptions
{
    /// <summary>
    /// Sistemdeki iş kuralları (Business Rules) ihlal edildiğinde fırlatılan özel hata (Exception) sınıfıdır.
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
        }  
    }
}
