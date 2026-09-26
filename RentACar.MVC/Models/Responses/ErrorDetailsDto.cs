namespace RentACar.MVC.Models.Responses
{
    public class ErrorDetailsDto
    {
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
        public IEnumerable<string>? ValidationErrors { get; set; }
    }
}
