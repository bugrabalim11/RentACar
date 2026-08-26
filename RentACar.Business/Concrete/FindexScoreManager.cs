using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;

namespace RentACar.Business.Concrete
{
    public class FindexScoreManager : IFindexScoreService
    {
        private readonly IFindexScoreService _findexScoreService;

        public FindexScoreManager(IFindexScoreService findexScoreService)
        {
            _findexScoreService = findexScoreService;
        }

        public int GetScoreByCustomerId(int customerId)
        {
            Random random = new Random();
            int result = random.Next(0, 1901); // 1900 dahil olması için 1901 yazarız.
            return result;
        }
    }
}
