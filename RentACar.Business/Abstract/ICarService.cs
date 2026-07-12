using RentACar.Core.Utilities.Results;
using RentACar.Dtos.CarDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface ICarService
    {
        // 1. Liste Dönerken (Join'li veriler bu kutuya girecek)
        Task<IDataResult<List<CarListDto>>> GetAllAsync();

        // 2. Tekil Dönerken
        Task<IDataResult<CarListDto>> GetByIdAsync(int id);

        // 3. Ekle, Sil, Güncelle işlemleri sadece boş kargo kutusu (IResult) döner
        Task<IResult> AddAsync(CarAddDto carAddDto);
        Task<IResult> UpdateAsync(CarUpdateDto carUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
