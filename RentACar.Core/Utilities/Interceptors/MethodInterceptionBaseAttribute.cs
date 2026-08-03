using Castle.DynamicProxy;

namespace RentACar.Core.Utilities.Interceptors
{
    // Class, Method vs. tepesine yazılabilsin (AttributeTargets)
    // Birden fazla eklenebilsin (AllowMultiple)
    // Miras alınan sınıflarda da geçerli olsun (Inherited)
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]


    // Abstract yapıyoruz ki tek başına kullanılamasın, sadece diğer ajanlara(Aspect) ata(Base) olsun.
    // IInterceptor: Castle.DynamicProxy'den gelir. "Ben bir araya giren ajanım" demektir.
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        // Öncelik sırası (Örn: Önce Validation çalışsın, sonra Transaction çalışsın demek için)
        public int Priority { get; set; }


        // IInterceptor arayüzünün zorunlu kıldığı metot. 
        // Intercept = Yakala! Metot tam çalışacakken ajan buraya düşer.
        // IInvocation: O an havada yakalanan metot (Örn: AddAsync) ve onun içindeki parametrelerdir.
        public virtual void Intercept(IInvocation invocation)
        {
            // İçi boş. Çünkü bu sadece Ata sınıf. 
            // Asıl işi (Transaction veya Validation), bunu miras alan çocuklar (Aspect'ler) dolduracak.
        }
    }
}
