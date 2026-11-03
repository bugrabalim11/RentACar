using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class OfficeRepository : Repository<Office>, IOfficeRepository
    {
        public OfficeRepository(RentACarContext context) : base(context)
        {
        }
    }
}
