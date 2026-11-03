using RentACar.DataAccess.Abstract;
using RentACar.Entities.Concrete;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class ContactMessageRepository : Repository<ContactMessage>, IContactMessageRepository
    {
        public ContactMessageRepository(RentACarContext context) : base(context)
        {
        }
    }
}
