using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        // Sadece data ve mesaj vermek istersek 
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
        }

        // Sadece data vermek istersek
        public ErrorDataResult(T data) : base(data, false)
        {
        }

        // Sadece mesaj vermek istersek (Data null/default gider)
        public ErrorDataResult(string message) : base(default!, false, message)
        {
        }

        // Hiçbir şey vermeden sadece başarısız demek istersek
        public ErrorDataResult() : base(default!, false)
        {
        }
    }
}
