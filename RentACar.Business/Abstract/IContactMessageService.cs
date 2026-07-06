using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Abstract
{
    public interface IContactMessageService
    {
        Task<List<ContactMessage>> GetAllAsync();
        Task<ContactMessage?> GetByIdAsync(int id);
        Task AddAsync(ContactMessage contactMessage);
        Task UpdateAsync(ContactMessage contactMessage);
        Task DeleteAsync(ContactMessage contactMessage);
    }
}
