using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    public class Result : IResult
    {
        // 1. Durum: Sadece 'Başarı Durumu' göndermek isteyebiliriz.
        public Result(bool success)
        {
            Success = success;
        }

        // 2. Durum: Hem 'Başarı Durumu' hem de 'Mesaj' göndermek isteyebiliriz.
        // ": this(success)" kodu şu demek: "Bu metot çalıştığında, git önce yukarıdaki tek parametreli
        // constructor'ı da çalıştır, kodu tekrar etme."
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }

        // Dışarıdan sadece okunabilir (get) özellikleri sözleşmeden (IResult) aldık
        public bool Success { get; }
        public string? Message { get; }
    }
}
