using Castle.DynamicProxy;

namespace RentACar.Core.Utilities.Interceptors
{
    /// <summary>
    /// Aspect'ler (Interceptor) için temel Attribute (Mühür) altyapısını sağlayan ve 
    /// Castle.DynamicProxy'nin IInterceptor arayüzünü uygulayan soyut temel sınıftır.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class MethodInterceptionBaseAttribute : Attribute, IInterceptor
    {
        public int Priority { get; set; }
        public virtual void Intercept(IInvocation invocation)
        {
        }
    }
}
