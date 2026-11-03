namespace RentACar.Core.Utilities.Results
{
    /// <summary>
    /// İşlem sonucu, başarı durumu ve bilgilendirme mesajını taşıyan temel sözleşmedir.
    /// </summary>
    public interface IResult
    {
        bool Success { get; }
        string? Message { get; }
    }
}
