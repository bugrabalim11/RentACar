using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(RentACarContext context) : base(context)
        {
        }
    }
}
