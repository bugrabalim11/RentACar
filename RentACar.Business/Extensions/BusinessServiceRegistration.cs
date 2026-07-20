using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RentACar.Business.Abstract;
using RentACar.Business.Concrete;
using RentACar.DataAccess.Abstract;
using RentACar.DataAccess.Concrete;
using RentACar.DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Reflection;
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

            // Business katmanındaki tüm Validator sınıflarını tara ve otomatik olarak sisteme kaydet
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Business kayıtları
            services.AddScoped<ICarService, CarManager>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<IBrandService, BrandManager>();
            services.AddScoped<IColorService, ColorManager>();
            services.AddScoped<ICustomerService, CustomerManager>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOfficeService, OfficeManager>();
            services.AddScoped<IOfficeRepository, OfficeRepository>();
            services.AddScoped<IRentalService, RentalManager>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<IContactMessageService, ContactMessageManager>();
            services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
            services.AddScoped<IContactInfoService, ContactInfoManager>();
            services.AddScoped<IContactInfoRepository, ContactInfoRepository>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
