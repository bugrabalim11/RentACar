using Castle.DynamicProxy;
using System.Reflection;

namespace RentACar.Core.Utilities.Interceptors
{
    /// <summary>
    /// Sınıf (Class) ve Metot seviyesindeki Aspect'leri okuyup, 
    /// Priority (Öncelik) sırasına göre çalıştırılmak üzere sıralayan ve IoC konteynerine ileten seçici sınıftır.
    /// </summary>
    public class AspectInterceptorSelector : IInterceptorSelector
    {
        public IInterceptor[]? SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
        {
            var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>(true).ToList();
            var methodAttributes = type.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray())?.GetCustomAttributes<MethodInterceptionBaseAttribute>(true);

            if (methodAttributes != null)
            {
                classAttributes.AddRange(methodAttributes);
            }

            return classAttributes.OrderBy(x => x.Priority).ToArray();
        }
    }
}
