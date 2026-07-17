using RentACar.Core.Utilities.Results;
using RentACar.Dtos.OfficeDtos;
using RentACar.Dtos.RentalDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IRentalService
    {
        Task<IDataResult<List<RentalListDto>>> GetAllAsync();
        Task<IDataResult<RentalListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(RentalAddDto rentalAddDto);
        Task<IResult> UpdateAsync(RentalUpdateDto rentalUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
