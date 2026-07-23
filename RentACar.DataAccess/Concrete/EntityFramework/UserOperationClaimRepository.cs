using Microsoft.EntityFrameworkCore;
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

        public async Task<List<UserOperationClaimDetailDto>> GetClaimDetailsAsync()
        {
            // 1. Haberci (Sorgu) asıl Lonca Kayıt Defterini açıyor. 
            // uoc: UserOperationClaims (Kimin hangi rütbeye sahip olduğunu tutan sayısal kayıtlar)
            var result = from uoc in _context.UserOperationClaims

                         // 2. Haberci sağ eline Oyuncular Kitabını (Users) alıyor ve Lonca defterindeki UserId ile hizalıyor.
                         // "Lonca defterindeki bu numara, Oyuncular kitabındaki kime ait?"
                         join u in _context.Users
                         on uoc.UserId equals u.Id

                         // 3. Haberci sol eline Rozetler Kitabını (OperationClaims) alıyor ve hizalıyor.
                         // "Lonca defterindeki bu rütbe numarası, Rozetler kitabındaki hangi unvana denk geliyor?"
                         join oc in _context.OperationClaims
                         on uoc.OperationClaimId equals oc.Id

                         // 4. Bütün kitaplar hizalandı! Şimdi haberci temiz bir A4 kağıdı (DTO - Sunum Tabağı) çıkarıyor.
                         // Sadece ekranda göstereceğimiz, müşterinin (veya oyun motorunun) anlayacağı bilgileri seçiyoruz (Select).
                         select new UserOperationClaimDetailDto
                         {
                             Id = uoc.Id,  // Atamanın kendi kayıt numarası
                             UserId = uoc.UserId,
                             UserFullName = u.FirstName + " " + u.LastName,   // İsim ve soyismi tek bir satırda, jilet gibi birleştirdik
                             OperationClaimId = oc.Id,
                             ClaimName = oc.Name  // Rütbenin gerçek adı (Örn: "Admin" veya "Şövalye")
                         };

            // 5. Hazırlanan bu temiz A4 kağıtlarını bir dosya haline getirip (Liste) vezneye (Manager'a) gönderiyoruz.
            return await result.ToListAsync();
        }
    }
}
