namespace RentACar.MVC.Models.Responses
{
    /// <summary>
    /// API'den dönen standart IDataResult / IResult yanıtlarını MVC tarafında (Client) karşılayan evrensel Generic sarmalayıcı (Wrapper) modeldir.
    /// </summary>
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T Data { get; set; } = default!;
    }
}
