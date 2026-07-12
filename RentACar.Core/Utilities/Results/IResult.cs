using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Results
{
    // Temel void (veri döndürmeyen) işlemler için başlangıç şablonu
    public interface IResult
    {
        // İşlem başarılı mı, başarısız mı? (true/false)
        bool Success { get; }

        // Kullanıcıya veya sisteme verilecek mesaj ("Araç eklendi" vb.)
        string? Message { get; }


        // Not: Sadece get yazıyoruz çünkü bu kutu bir kere oluşturulduğunda içindeki
        // mesaj ve durum sonradan değiştirilemesin istiyoruz, yani veriyi korumaya (encapsulation) alıyoruz.
    }
}
