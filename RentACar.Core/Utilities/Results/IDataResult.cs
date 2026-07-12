using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    // T: Hangi tipi (Car, Brand, List<Color> vb.) döndüreceğimizi temsil eder.
    // IResult'tan miras alıyoruz çünkü DataResult'ın da Success ve Message'ı olmalı!
    public interface IDataResult<T> : IResult
    {
        // İşlem sonucunda döndürülecek veri
        T Data { get; }
    }
}
