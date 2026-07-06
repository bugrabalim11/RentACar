using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class ContactInfoManager : IContactInfoService
    {
        private readonly IRepository<ContactInfo> _contactInfoRepository;

        public ContactInfoManager(IRepository<ContactInfo> contactInfoRepository)
        {
            _contactInfoRepository = contactInfoRepository;
        }

        public async Task AddAsync(ContactInfo contactInfo)
        {
            await _contactInfoRepository.AddAsync(contactInfo);
        }

        public async Task DeleteAsync(ContactInfo contactInfo)
        {
            await _contactInfoRepository.DeleteAsync(contactInfo);
        }

        public async Task<List<ContactInfo>> GetAllAsync()
        {
            return await _contactInfoRepository.GetAllAsync();
        }

        public Task<ContactInfo?> GetByIdAsync(int id)
        {
            return _contactInfoRepository.GetAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(ContactInfo contactInfo)
        {
            await _contactInfoRepository.UpdateAsync(contactInfo);
        }
    }
}
