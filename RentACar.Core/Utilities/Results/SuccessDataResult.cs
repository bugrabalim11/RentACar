using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        // Sadece data ve mesaj vermek istersek (En çok bunu kullanacağız)
        public SuccessDataResult(T data, string message) : base(data, true, message)
        {
        }

        // Sadece data vermek istersek
        public SuccessDataResult(T data) : base(data, true)
        {
        }

        // Sadece mesaj vermek istersek (Data kısmına T'nin varsayılan boş değerini 'default' atarız)
        public SuccessDataResult(string message) : base(default!, true, message)
        {
        }

        // Hiçbir şey vermeden sadece başarılı demek istersek
        public SuccessDataResult() : base(default!, true)
        {
        }
    }
}
