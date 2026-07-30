using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class CarImageRepository : Repository<CarImage>, ICarImageRepository
    {
        public CarImageRepository(RentACarContext context) : base(context)
        {
        }
    }
}
