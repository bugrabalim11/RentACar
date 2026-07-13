using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class OfficeRepository : Repository<Office>, IOfficeRepository
    {
        public OfficeRepository(RentACarContext context) : base(context)
        {
        }
    }
}
