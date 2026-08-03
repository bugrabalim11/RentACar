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
            // TransactionScope: C#'ın kendi içinde var olan "Koruma Balonu" (Zaman Makinesi)
            using (TransactionScope transactionScope = new TransactionScope())
            {
                try
                {
                    // 1. Adım: "İçeri gir ve asıl metodu (Örn: AddAsync) çalıştır" diyoruz.
                    invocation.Proceed();

                    // 2. Adım: Eğer üst satırda sistem patlamadıysa ve buraya kadar geldiysek,
                    // Her şey kusursuz çalışmış demektir. Balonu onayla ve veritabanına kalıcı olarak yaz! (Commit)
                    transactionScope.Complete();
                }
                catch(System.Exception)
                {
                    // 3. Adım: Eğer invocation.Proceed() çalışırken bir yerde hata fırlarsa,
                    // Sistem buraya (catch) düşer. Balonu patlat (Dispose) ve yapılan her işlemi geri al! (Rollback)
                    transactionScope.Dispose();

                    // Hatayı yutma, sisteme geri fırlat ki API'miz "500 Internal Server Error" verebilsin.
                    throw;
                }
            }
        }
    }
}
