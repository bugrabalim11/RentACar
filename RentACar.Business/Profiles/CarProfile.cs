using AutoMapper;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Profiles
{
    // Profile sınıfından miras aldığımıza dikkat et (AutoMapper kütüphanesinden gelir)
    public class CarProfile : Profile
    {
        public CarProfile()
        {
            // 1. KURAL: Veritabanından gelen Car nesnesini, müşteriye gidecek CarListDto'ya çevir
            CreateMap<Car, CarListDto>()
                // DTO'daki BrandName alanını, Entity'deki Brand.Name'den al
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                // DTO'daki ColorName alanını, Entity'deki Color.Name'den al
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name));

            // 2. KURAL: Kullanıcıdan gelen CarAddDto'yu (içinde Id yok), veritabanına kaydedilecek Car nesnesine çevir
            CreateMap<CarAddDto, Car>();

            // 3. KURAL: Kullanıcıdan gelen Update DTO'sunu, veritabanına gidecek Car nesnesine çevir
            CreateMap<CarUpdateDto, Car>();
        }
    }
}
