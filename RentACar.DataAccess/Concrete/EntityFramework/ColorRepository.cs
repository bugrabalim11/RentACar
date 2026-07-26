using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        public ColorRepository(RentACarContext context) : base(context)
        {
        }
    }
}
