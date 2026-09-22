namespace RentACar.Core.Utilities.Results
{
    public class ErrorResult : Result
    {
        // base'e "false" yolluyoruz çünkü bu bir hata sonucu!
        public ErrorResult(string message) : base(false, message)
        {
        }
        public ErrorResult() : base(false)
        {
        }
    }
}
