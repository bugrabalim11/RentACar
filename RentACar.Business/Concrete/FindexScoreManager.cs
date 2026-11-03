using RentACar.Business.Abstract;

namespace RentACar.Business.Concrete
{
    public class FindexScoreManager : IFindexScoreService
    {
        public int GetScoreByCustomerId(int customerId)
        {
            Random random = new Random();
            int result = random.Next(0, 1901); 
            return result;
        }
    }
}
