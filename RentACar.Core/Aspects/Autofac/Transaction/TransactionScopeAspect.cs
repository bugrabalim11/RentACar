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
            // Zaman makinesini başlat
            var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                // 1. İşçiyi odaya sok ve çalıştır
                invocation.Proceed();

                // 2. KONTROL: Metot Asenkron mu? (Task döndürüyor mu?)
                if (invocation.ReturnValue is Task task)
                {
                    // DOĞRU OLAN: Kutunun şekline değil, metodun Orijinal Sözleşmesine (Signature) bakıyoruz!
                    // Sözleşme asla yalan söylemez, direkt 'Task<IDataResult<int>>' olarak döner.
                    var returnType = invocation.Method.ReturnType;

                    // Eğer kutu VIP bir kutuysa (Yani Task<T> gibi Generic bir tipse)
                    if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        // Kutunun içindeki asıl malzemenin tipini bul (Örn: IDataResult<int>)
                        var resultType = returnType.GetGenericArguments()[0];

                        // Reflection Sihri: Ajanın içindeki 'HandleAsyncWithResult' metodunu bul ve ona bu malzemeyi öğret
                        var method = typeof(TransactionScopeAspect)
                            .GetMethod(nameof(HandleAsyncWithResult), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                            ?.MakeGenericMethod(resultType);

                        // Özel metodumuzu çalıştırıp VIP kutuyu teslim ediyoruz
                        invocation.ReturnValue = method?.Invoke(this, new object[] { task, transactionScope });
                    }
                    else
                    {
                        // Normal Kutu (Düz Task dönüyorsa) eski sistem çalışır
                        invocation.ReturnValue = HandleAsync(task, transactionScope);
                    }
                }
                else
                {
                    // Metot Senkron ise (Düz void veya int dönüyorsa) eski sistem çalışır
                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
            }
            catch (System.Exception)
            {
                // İşçi daha çalışmaya başlamadan (Proceed anında) patlarsa
                transactionScope.Dispose();
                throw;
            }
        }

        // AJANIN YENİ VIP ASENKRON TAKİP CİHAZI
        private async Task<T> HandleAsyncWithResult<T>(Task task, TransactionScope transactionScope)
        {
            try
            {
                // İşçinin işini arka planda asenkron olarak bitirmesini bekle
                var genericTask = (Task<T>)task;
                var result = await genericTask;

                // Hata çıkmadıysa zaman makinesini onayla
                transactionScope.Complete();

                // VIP kutuyu içindeki veriyle (result) beraber teslim et!
                return result;
            }
            finally
            {
                // İşlem bitse de, hata da fırlasa makineyi temizle
                transactionScope.Dispose();
            }
        }

        // AJANIN STANDART ASENKRON TAKİP CİHAZI (Bunu silmiştik, geri ekliyoruz!)
        // Eğer metot geriye IDataResult gibi bir VIP kutu değil de, düz Task dönüyorsa bu çalışır.
        private async Task HandleAsync(Task task, TransactionScope transactionScope)
        {
            try
            {
                await task;
                transactionScope.Complete();
            }
            finally
            {
                transactionScope.Dispose();
            }
        }
    }
}
