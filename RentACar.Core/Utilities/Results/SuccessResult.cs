using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    // SuccessResult aslında bir Result'tır. (Kalıtım / Inheritance)
    public class SuccessResult : Result
    {
        // "base(true, message)" -> Miras aldığımız Result sınıfına "true" ve mesajı yolla.
        public SuccessResult(string message) : base(true, message)
        {
        }

        // Bazen mesaj vermek istemeyiz, sadece başarılı deriz.
        public SuccessResult() : base(true)
        {
        }
    }
}
