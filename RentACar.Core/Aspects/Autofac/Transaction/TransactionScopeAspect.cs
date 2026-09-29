using Castle.DynamicProxy;
using RentACar.Core.Utilities.Interceptors;
using System.Transactions;  // Zaman makinesinin motoru buradan gelir

namespace RentACar.Core.Aspects.Autofac.Transaction
{
    // Ajanımızın beyni olan MethodInterception'dan miras alıyoruz
    public class TransactionScopeAspect : MethodInterception
    {
        // Intercept: Havada yakala! Metot tam çalışacakken ajan araya giriyor.
        public override void Intercept(IInvocation invocation)
        {
            // TransactionScopeAsyncFlowOption.Enabled : 
            // Zaman makinesinin farklı asenkron işçiler (Thread'ler) arasında kopmadan devam etmesini sağlar!
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // 1. İşçiyi odaya sok ve çalıştır (Burada işçi bekleme odasına -await- geçebilir)
                    invocation.Proceed();

                    // 2. SABIRSIZ AJAN İÇİN KONTROL NOKTASI!
                    // Eğer dönen sonuç bir "Task" (Yani asenkron bir görev) ise:
                    if (invocation.ReturnValue is System.Threading.Tasks.Task returnValueTask)
                    {
                        // Ajan'a diyoruz ki: "İçerideki asenkron işlem %100 bitene kadar burada BEKLE!"
                        // (Bunu demezsek Ajan hemen aşağı inip işlemi onaylar ve kaçar)
                        returnValueTask.Wait();
                    }

                    // 3. İşçi (veya işçiler) işini tamamen, hatasız bitirdi. Artık zaman makinesini onaylayabilirsin!
                    transactionScope.Complete();
                }
                catch (System.Exception)
                {
                    // 4. Adım: Eğer invocation.Proceed() çalışırken bir yerde hata fırlarsa,
                    // Sistem buraya (catch) düşer. Balonu patlat (Dispose) ve yapılan her işlemi geri al! (Rollback)
                    transactionScope.Dispose();

                    // Hatayı yutma, sisteme geri fırlat ki API'miz "500 Internal Server Error" verebilsin.
                    throw;
                }
            }
        }
    }
}
