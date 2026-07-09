using Microsoft.Extensions.DependencyInjection;
using RentACar.Business.Abstract;
using RentACar.Business.Concrete;
using RentACar.DataAccess.Abstract;
using RentACar.DataAccess.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Extensions
{
    // Sınıf static olmalı!
    public static class BusinessServiceRegistration
    {
        // IServiceCollection'ı genişletiyoruz (Extension Method)
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            // DataAccess kayıtları
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Business kayıtları
            services.AddScoped<ICarService, CarManager>();
            services.AddScoped<IBrandService, BrandManager>();

            return services;
        }
    }
}
