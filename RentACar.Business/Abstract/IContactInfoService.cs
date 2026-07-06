using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IContactInfoService
    {
        Task<List<ContactInfo>> GetAllAsync();
        Task<ContactInfo?> GetByIdAsync(int id);
        Task AddAsync(ContactInfo contactInfo);
        Task UpdateAsync(ContactInfo contactInfo);
        Task DeleteAsync(ContactInfo contactInfo);
    }
}
