using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(RentACarContext context) : base(context)
        {
        }
    }
}
