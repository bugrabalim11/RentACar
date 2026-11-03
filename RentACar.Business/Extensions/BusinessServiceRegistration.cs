using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentACar.DataAccess.Abstract;
using RentACar.DataAccess.Concrete.EntityFramework;
using System.Reflection;

namespace RentACar.Business.Extensions
{
    /// <summary>
    /// Business katmanına ait servislerin, genel repository'lerin ve FluentValidation kurallarının
    /// .NET'in yerleşik IoC konteynerine (IServiceCollection) kaydedilmesini sağlar.
    /// </summary>
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
