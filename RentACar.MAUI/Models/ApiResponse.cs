namespace RentACar.MAUI.Models
{
    /// <summary>
    /// Web API'den dönen standart IDataResult (Data, Success, Message) yapısını karşılar.
    /// </summary>
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
