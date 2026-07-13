using RentACar.Core.Utilities.Results;
using RentACar.Dtos.ColorDtos;
using RentACar.Dtos.CustomerDtos;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface ICustomerService
    {
        Task<IDataResult<List<CustomerListDto>>> GetAllAsync();
        Task<IDataResult<CustomerListDto>> GetByIdAsync(int id);
        Task<IResult> AddAsync(CustomerAddDto customerAddDto);
        Task<IResult> UpdateAsync(CustomerUpdateDto customerUpdateDto);
        Task<IResult> DeleteAsync(int id);
    }
}
