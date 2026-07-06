using RentACar.Business.Abstract;
using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerManager(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task AddAsync(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.NationalIdentity) || customer.NationalIdentity.Length != 11)
            {
                throw new ArgumentException("Geçerli bir T.C. kimlik numarası girilmelidir (11 hane)!");
            }

            await _customerRepository.AddAsync(customer);
        }

        public async Task DeleteAsync(Customer customer)
        {
            await _customerRepository.DeleteAsync(customer);
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            // Müşteriyi Id'sine göre getiren LINQ filtresi
            return await _customerRepository.GetAsync(c => c.Id == id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await _customerRepository.UpdateAsync(customer);
        }
    }
}
