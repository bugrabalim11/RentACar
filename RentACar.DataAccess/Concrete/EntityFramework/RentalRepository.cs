using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class RentalRepository : Repository<Rental>, IRentalRepository
    {
        public RentalRepository(RentACarContext context) : base(context)
        {
        }
    }
}
