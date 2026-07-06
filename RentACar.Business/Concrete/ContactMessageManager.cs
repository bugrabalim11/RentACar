using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class ContactMessageManager : IContactMessageService
    {
        private readonly IRepository<ContactMessage> _contactMessageRepository;

        public ContactMessageManager(IRepository<ContactMessage> contactMessageRepository)
        {
            _contactMessageRepository = contactMessageRepository;
        }

        public async Task AddAsync(ContactMessage contactMessage)
        {
            await _contactMessageRepository.AddAsync(contactMessage);
        }

        public async Task DeleteAsync(ContactMessage contactMessage)
        {
            await _contactMessageRepository.DeleteAsync(contactMessage);
        }

        public async Task<List<ContactMessage>> GetAllAsync()
        {
            return await _contactMessageRepository.GetAllAsync();
        }

        public async Task<ContactMessage?> GetByIdAsync(int id)
        {
            return await _contactMessageRepository.GetAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(ContactMessage contactMessage)
        {
            await _contactMessageRepository.UpdateAsync(contactMessage);
        }
    }
}
