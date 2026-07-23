using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserOperationClaimDtos;
using RentACar.DataAccess.Abstract;

namespace RentACar.DataAccess.Concrete.EntityFramework
{
    public class UserOperationClaimRepository : Repository<UserOperationClaim>, IUserOperationClaimRepository
    {
        private readonly RentACarContext _context;
        public UserOperationClaimRepository(RentACarContext context) : base(context)
        {
            _context = context;
        }

        public List<UserOperationClaimDetailDto> GetClaimDetails()
        {
            // LINQ ile tabloları dikiyoruz (Join)
            var result = from uoc in _context.UserOperationClaims  // Ana tablomuzdan başlıyoruz

                         // User tablosu ile birleştir: "Senin UserId'in ile User tablosundaki Id eşleşsin"
                         join u in _context.Users
                         on uoc.UserId equals u.Id

                         // OperationClaim tablosu ile birleştir: "Senin OperationClaimId'in ile Claim tablosundaki Id eşleşsin"
                         join oc in _context.OperationClaims
                         on uoc.OperationClaimId equals oc.Id

                         // Malzemeler birleşti! Şimdi bunları yeni tabağımıza (DetailDto) yerleştir (Select)
                         select new UserOperationClaimDetailDto
                         {
                             Id = uoc.Id,
                             UserId = uoc.UserId,
                             UserFullName = u.FirstName + " " + u.LastName,
                             OperationClaimId = oc.Id,
                             ClaimName = oc.Name
                         };
            // Çıkan sonucu liste yap ve gönder
            return result.ToList();
        }
    }
}
