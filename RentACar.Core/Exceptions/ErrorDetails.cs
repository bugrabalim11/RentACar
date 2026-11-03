namespace RentACar.Core.Exceptions
{
    /// <summary>
    /// API genelinde fırlatılan tüm hataların, istemciye (Client/UI) tek tipte (standart) 
    /// gönderilmesini sağlayanhata veri modelidir.
    /// </summary>
    public class ErrorDetails
    {
        public string Message { get; set; } = null!;
        public int StatusCode { get; set; }
        public IEnumerable<string>? ValidationErrors { get; set; } 
    }
}
