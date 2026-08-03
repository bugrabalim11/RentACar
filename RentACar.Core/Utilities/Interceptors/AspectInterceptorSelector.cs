using Castle.DynamicProxy;
using System.Reflection;

namespace RentACar.Core.Utilities.Interceptors
{
    // IInterceptorSelector: Castle.DynamicProxy'den gelir. "Ben araya giren ajanları seçen trafik polisiyim" demektir.
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[]? SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            // 1. Sınıfın (Class) tepesindeki mühürleri (Attribute) listele
            // type.GetCustomAttributes: O an çalışan sınıfın tepesindeki mühürleri okur.
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();

            // 2. O an çalışan Metodun (Method) tepesindeki mühürleri listele
            // type.GetMethod() ile tekrar aramıyoruz, postacının verdiği 'method' kolisini direkt açıyoruz.
            var methodAttributes = method.GetCustomAttributes<MethodInterceptionBaseAttribute>(true);

            // 3. Sınıftaki ve metottaki mühürleri aynı listede birleştir (Polis listeyi hazırlıyor)
            if (methodAttributes != null)
            {
                classAttributes.AddRange(methodAttributes);
            }

            // (Opsiyonel Vizyon: Murat Yücedağ burada bazen "Tüm sisteme otomatik Loglama ekle" gibi varsayılan kurallar da yazar. Şimdilik MVP'de tutuyoruz.)

            // 4. Listeyi Priority (Öncelik) değerine göre sırala ve diziye çevirip Autofac'e teslim et.
            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
