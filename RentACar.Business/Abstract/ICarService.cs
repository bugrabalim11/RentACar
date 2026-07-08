using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface ICarService
    {
        // Artık List<Car> değil, List<CarListDto> dönüyoruz! Tencereleri gizledik.
        Task<List<CarListDto>> GetAllAsync();

        Task<CarListDto?> GetByIdAsync(int id); // Bulamazsa null döner (T? mantığı)

        // Dışarıdan Car değil, CarAddDto alıyoruz! (Çünkü Id'yi veritabanı verecek)
        Task AddAsync(CarAddDto carAddDto);
        Task<bool> UpdateAsync(CarUpdateDto carUpdateDto);
        Task<bool> DeleteAsync(int id);
    }
}
