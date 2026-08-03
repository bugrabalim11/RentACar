using Castle.DynamicProxy;

namespace RentACar.Core.Utilities.Interceptors
{
    // Az önce yazdığımız Ata sınıftan miras alıyoruz.
    // Bu sınıf bizim asıl "Ajanımızın Beyni" olacak.
    public abstract class MethodInterception : MethodInterceptionBaseAttribute
    {
        // Metot çalışmadan HEMEN ÖNCE ne yapılsın? (Örn: Validation - Kapıdan girerken kılık kıyafet kontrolü)
        protected virtual void OnBefore(IInvocation invocation) { }

        // Metot BİTTİKTEN SONRA ne yapılsın? (Örn: Loglama - Çıkış kapısında kayıt tutma)
        protected virtual void OnAfter(IInvocation invocation) { }

        // Metot HATA VERİRSE ne yapılsın? (Örn: Transaction - Her şeyi iptal et, Rollback!)
        protected virtual void OnException(IInvocation invocation, System.Exception e) { }

        // Metot BAŞARIYLA TAMAMLANIRSA ne yapılsın? (Örn: Transaction - Her şeyi onayla, Commit!)
        protected virtual void OnSuccess(IInvocation invocation) { }

        // Ata sınıftaki "Intercept" (Yakala) metodunu eziyoruz (override) ve kendi yaşam döngümüzü kuruyoruz.
        // IInvocation: O an havada yakalanan metot (Örn: AddAsync)
        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;

            // 1. Adım: Asıl metot çalışmadan önce yapılması gerekenleri tetikle (OnBefore)
            OnBefore(invocation);

            try
            {
                // 2. Adım: ASIL METODU ÇALIŞTIR! (invocation.Proceed() = İçeri gir, işini yap)
                invocation.Proceed();
            }
            catch (Exception e)
            {
                isSuccess = false;
                // 3. Adım: Hata çıkarsa, OnException içindeki kuralları çalıştır
                OnException(invocation, e);
                throw; // Hatayı yutma, sisteme geri fırlat ki patladığını bilelim
            }
            finally
            {
                // 4. Adım: Hata çıkmadıysa (isSuccess true kaldıysa), OnSuccess çalışsın
                if (isSuccess)
                {
                    OnSuccess(invocation);
                }
            }
            // 5. Adım: Hata çıksa da çıkmasa da en son burası çalışır (OnAfter)
            OnAfter(invocation);
        }
    }
}
