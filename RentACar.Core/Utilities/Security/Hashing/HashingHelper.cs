using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Core.Utilities.Security.Hashing
{
    public static class HashingHelper
    {
        // 1. Kayıt Olurken (Register): Şifreyi ver, sana Tuz ve Kıymayı versin.
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // Kıyma makinesini (HMACSHA512) Kullan-At laboratuvarında (using) çalıştırıyoruz.
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // 1. Makine çalışır çalışmaz bize tamamen rastgele, eşsiz bir TUZ verir. Bunu Salt kutusuna koyuyoruz.
                passwordSalt = hmac.Key;


                // 2. Kullanıcının girdiği string şifreyi (Eti), byte'a (Demire) çevirip makineye (ComputeHash) atıyoruz.
                // Çıkan kıymayı da Hash kutusuna koyuyoruz.
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


        // 2. Giriş Yaparken (Login): Şifreyi, veritabanındaki Tuz ve Kıyma ile karşılaştır.
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // Makineyi bu sefer rastgele değil, veritabanından gelen Tuz ile çalıştırıyoruz!
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                // Adamın girdiği şifreyi makineye atıp yeni bir kıyma (computedHash) üretiyoruz.
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                // Çıkan kıymayı, veritabanındaki kıyma ile harf harf (byte byte) karşılaştırıyoruz.
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        // Tek bir uyuşmazlık bile varsa sahtekardır!
                        return false;
                    }
                }
            }
            // Tüm döngü bitti ve sorun çıkmadıysa, şifre %100 doğrudur!
            return true;
        }
    }
}
