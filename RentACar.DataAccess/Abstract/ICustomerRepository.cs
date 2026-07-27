using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Abstract
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Customer>> GetCustomersWithDetailsAsync();

        Task<Customer?> GetCustomerWithDetailsAsync(int id);
    }
}
