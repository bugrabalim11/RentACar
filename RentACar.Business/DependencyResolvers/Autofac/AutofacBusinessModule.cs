using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using RentACar.Core.Utilities.Interceptors;
using System.Reflection;
using Module = Autofac.Module; 

namespace RentACar.Business.DependencyResolvers.Autofac
{
    /// <summary>
    /// Projenin bağımlılık enjeksiyonu (IoC) yapılandırmasını Autofac üzerinden yönetir.
    /// Business, DataAccess ve Core katmanlarındaki servisleri, arayüzleri 
    /// ve Aspect-Oriented Programming (AOP) interceptor'larını sisteme kaydeder.
    /// </summary>
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var businessAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(businessAssembly)
                   .AsImplementedInterfaces()
                   .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                   {
                       Selector = new AspectInterceptorSelector()
                   })
                   .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(businessAssembly)
                   .Where(t => t.Name.EndsWith("BusinessRules"))
                   .AsSelf()
                   .InstancePerLifetimeScope(); 

            var dataAccessAssembly = typeof(RentACar.DataAccess.Concrete.EntityFramework.CarRepository).Assembly;

            builder.RegisterAssemblyTypes(dataAccessAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            var coreAssembly = typeof(RentACar.Core.Utilities.Helpers.FileHelper.FileHelperManager).Assembly;

            builder.RegisterAssemblyTypes(coreAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }
    }
}
