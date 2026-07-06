using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IOfficeService
    {
        Task<List<Office>> GetAllAsync();
        Task<Office?> GetByIdAsync(int id);
        Task AddAsync(Office office);
        Task UpdateAsync(Office office);
        Task DeleteAsync(Office office);
    }
}
