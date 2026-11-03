namespace RentACar.Core.Utilities.Results
{
    /// <summary>
    /// İşlem sonucu ve mesajına ek olarak, geriye bir veri (Data) döndüren sözleşmedir.
    /// </summary>
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
