using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using RentACar.Business.Rules;
using RentACar.Core.Utilities.Interceptors;
using System.Reflection;
using Module = Autofac.Module; // Autofac'in Module sınıfını kullanacağımızı belirtiyoruz

namespace RentACar.Business.DependencyResolvers.Autofac
{
    /// <summary>
    /// Microsoft'un standart İnsan Kaynakları (IoC Container) yerine Autofac kullanarak bağımlılıkları yöneten şubedir.
    /// Business katmanındaki tüm sınıfları (Assembly) tarar, Interface'leri ile otomatik eşleştirir ve 
    /// AOP (Zaman Makinesi, Loglama vb.) standartlarına uygun olarak metotların kapısına ajanların (Interceptors) dikilmesini sağlar.
    /// </summary>
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // 1. BUSINESS BİNASI (Ajanlı/Interceptor'lı işçiler)
            var businessAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(businessAssembly)
                   .AsImplementedInterfaces()
                   .EnableInterfaceInterceptors(new ProxyGenerationOptions()
                   {
                       Selector = new AspectInterceptorSelector()
                   })
                   .InstancePerLifetimeScope();;

            // MÜFETTİŞİN YAKA KARTI (Interface'i olmadığı için AsSelf kullanıyoruz)
            // TODO: Bunları otomatik hale getir
            builder.RegisterType<RentalBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CarBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<BrandBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ColorBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CarImageBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<AuthBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<OperationClaimBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserOperationClaimBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<UserBusinessRules>().AsSelf().InstancePerLifetimeScope();

            // 2. DATA ACCESS BİNASI (Ajansız, düz veritabanı işçileri)
            // Eskiden C#'a bana CarRepository'nin yaşadığı binayı (Assembly) bul diye adres veriyorduk 
            // Şimdi CarRepository'nin tipine (Type) bak, o kimliğin üstünde zaten yaşadığı bina (Assembly) yazıyor, direkt oradan al diyoruz   
            var dataAccessAssembly = typeof(RentACar.DataAccess.Concrete.EntityFramework.CarRepository).Assembly;

            builder.RegisterAssemblyTypes(dataAccessAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();;

            // 3. CORE BİNASI (JwtHelper, FileHelper vb. ortak araçlar)
            // Core projesini bulabilmesi için, o projenin içinden FileHelperManager (veya JwtHelper) sınıfını adres gösteriyoruz.
            var coreAssembly = typeof(RentACar.Core.Utilities.Helpers.FileHelper.FileHelperManager).Assembly;

            builder.RegisterAssemblyTypes(coreAssembly)
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();;
        }
    }
}
