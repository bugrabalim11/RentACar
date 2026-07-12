using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    // T: Döneceğimiz veri tipi (Car, List<Brand> vs.)
    public class DataResult<T> : Result, IDataResult<T>
    {
        // 1. Durum: Data, Başarı Durumu ve Mesaj yollamak istersek
        public DataResult(T data, bool success, string message) : base(success, message)
        {
            Data = data;
        }

        // 2. Durum: Sadece Data ve Başarı Durumu yollamak istersek (Mesajsız)
        public DataResult(T data, bool success) : base(success)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
