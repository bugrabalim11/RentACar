using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class OfficeManager : IOfficeService
    {
        private readonly IRepository<Office> _officeRepository;

        public OfficeManager(IRepository<Office> officeRepository)
        {
            _officeRepository = officeRepository;
        }

        public async Task AddAsync(Office office)
        {
            await _officeRepository.AddAsync(office);
        }

        public async Task DeleteAsync(Office office)
        {
            await _officeRepository.DeleteAsync(office);
        }

        public async Task<List<Office>> GetAllAsync()
        {
            return await _officeRepository.GetAllAsync();
        }

        public async Task<Office?> GetByIdAsync(int id)
        {
            return await _officeRepository.GetAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Office office)
        {
            await _officeRepository.UpdateAsync(office);
        }
    }
}
