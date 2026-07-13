using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Abstract
{
    public interface ICustomerRepository : IRepository<Customer>
    {
    }
}
