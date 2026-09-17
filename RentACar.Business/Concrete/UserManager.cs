using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Entities.DTOs.UserDtos;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Hashing;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        public UserManager(IUserRepository userRepository, IMapper mapper, IUserOperationClaimRepository userOperationClaimRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userOperationClaimRepository = userOperationClaimRepository;
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingUser = await _userRepository.GetAsync(x => x.Id == id);
            if (existingUser == null)
            {
                return new ErrorResult("Silinecek kullanıcı bulunamadı.");
            }

            existingUser.IsDeleted = true;
            existingUser.DeletedDate = DateTime.UtcNow;
            await _userRepository.UpdateAsync(existingUser);
            return new SuccessResult("Kullanıcı başarıyla silindi.");
        }

        public async Task<IResult> RestoreAsync(int id)
        {
            var deletedUser = await _userRepository.GetAsync(x => x.Id == id && x.IsDeleted == true, ignoreQueryFilters: true);
            if (deletedUser == null)
            {
                return new ErrorResult("Geri Getirilecek kullanıcı bulunamadı.");
            }
            deletedUser.IsDeleted = false;
            deletedUser.DeletedDate = null;
            await _userRepository.UpdateAsync(deletedUser);
            return new SuccessResult("Kullanıcı başarıyla geri getirildi.");
        }

        public async Task<IDataResult<List<UserResultDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserResultDto>>(users);
            return new SuccessDataResult<List<UserResultDto>>(userDtos, "Kullanıcılar başarıyla listelendi.");
        }

        public async Task<IDataResult<List<UserResultForAdminDto>>> GetAllForAdminAsync()
        {
            var users = await _userRepository.GetAllAsync(ignoreQueryFilters: true);
            var userDtos = _mapper.Map<List<UserResultForAdminDto>>(users);
            return new SuccessDataResult<List<UserResultForAdminDto>>(userDtos, "Kullanıcılar başarıyla listelendi.");
        }

        public async Task<IDataResult<UserResultDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<UserResultDto>("Kullanıcı bulunamadı.");
            }

            var userDto = _mapper.Map<UserResultDto>(user);
            return new SuccessDataResult<UserResultDto>(userDto, "Kullancı başarıyla getirildi.");
        }

        public async Task<IDataResult<UserUpdateForAdminDto>> GetByIdForUpdateAsync(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<UserUpdateForAdminDto>("Kullanıcı bulunamadı.");
            }

            var operationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == user.Id);

            var userDto = _mapper.Map<UserUpdateForAdminDto>(user);

            // Şüpheli paket etiketini (if) kaldırdık, doğrudan atamayı çaktık!
            // KONTROL EDİLECEK ŞART? EVET İSE BURASI ÇALIŞIR: HAYIR İSE BURASI ÇALIŞIR
            userDto.OperationClaimId = (operationClaim != null) ? operationClaim.OperationClaimId : 0;

            return new SuccessDataResult<UserUpdateForAdminDto>(userDto, "Kullancı başarıyla getirildi.");
        }

        public async Task<IDataResult<UserResultDto>> GetMyProfile(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<UserResultDto>("Profil bulunamadı.");
            }

            var userDto = _mapper.Map<UserResultDto>(user);
            return new SuccessDataResult<UserResultDto>(userDto, "Profil başarıyla getirildi.");
        }

        public async Task<IResult> CreateForAdminAsync(UserCreateForAdminDto userCreateForAdminDto)
        {
            // 1. İş Kuralı Kontrolü: Bu e-posta daha önce alınmış mı diye güvenlik kurallarımıza soruyoruz.
            userCreateForAdminDto.Email = userCreateForAdminDto.Email.Trim().ToLower();
            IResult? result = BusinessRules.Run(await CheckIfEmailExistsAsync(userCreateForAdminDto.Email));
            if (result != null)
            {
                return result;
            }

            // 2. Güvenlik (Hashing): Gelen çıplak şifreyi blenderdan geçirip (Hash ve Salt) şifreli hale getiriyoruz.
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userCreateForAdminDto.Password, out passwordHash, out passwordSalt);

            // 3. Çevirmen (AutoMapper): Dışarıdan gelen bavulu (DTO), veritabanının anladığı gerçek Entity nesnesine dönüştürüyoruz.
            var user = _mapper.Map<User>(userCreateForAdminDto);

            // 4. Mühürleme: Şifre güvenlik verilerini ve varsayılan aktiflik durumunu nesneye manuel zerk ediyoruz.
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.IsDeleted = false;

            // 5. Kimlik Basımı: Kullanıcıyı depoya kaydediyoruz. Bu işlem bittiğinde EF Core, kullanıcıya otomatik bir Id atamış (user.Id) olacak.
            await _userRepository.AddAsync(user);

            // Admin kayıt yaparken rütbeyi doldurdu mu?
            if (userCreateForAdminDto.OperationClaimId.HasValue)
            {
                // 6. Yetki Kartı (Rol) Ataması: Yeni oluşan kimlik numarasıyla (user.Id),
                // Admin'in vitrinden seçtiği rol numarasını eşleştirip yetki tablosuna kaydediyoruz.
                UserOperationClaim userOperationClaim = new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = userCreateForAdminDto.OperationClaimId
                };
                await _userOperationClaimRepository.AddAsync(userOperationClaim);
                return new SuccessResult("Kullanıcı başarıyla eklendi ve rütbe ataması yapıldı.");
            }

            return new SuccessResult("Kullanıcı başarıyla eklendi.");
        }

        public async Task<IResult> UpdateForAdminAsync(UserUpdateForAdminDto userUpdateForAdminDto)
        {
            // 1. Kimlik Kontrolü: Güncellenmek istenen adam gerçekten veritabanında (depoda) var mı?
            userUpdateForAdminDto.Email = userUpdateForAdminDto.Email.Trim().ToLower();
            var existingUser = await _userRepository.GetAsync(x => x.Id == userUpdateForAdminDto.Id);
            if (existingUser == null)
            {
                return new ErrorResult("Güncellenecek kullanıcı bulunamadı.");
            }

            // 2. Güvenlik Duvarı: Adam e-postasını değiştiriyorsa, bu yeni e-posta sistemde başkası tarafından kullanılıyor mu?
            IResult? result = BusinessRules.Run(await CheckIfEmailExistsForUpdateAsync(userUpdateForAdminDto.Email, userUpdateForAdminDto.Id));
            if (result != null)
            {
                return result;
            }

            // 3. Kimlik Kartını Güncelleme: Dışarıdan gelen formdaki (DTO) yeni bilgileri, veritabanından çektiğimiz gerçek nesnenin üzerine yazıyoruz.
            // : Map(Kaynak, Hedef)
            _mapper.Map(userUpdateForAdminDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);

            // 4. Yetki Kartı (Rütbe) Operasyonu: Adamın mevcut bir rütbe kartı var mı diye arıyoruz.
            var operationClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == existingUser.Id);
            if (operationClaim != null)
            {
                // Durum A: Adamın zaten bir yetki kartı var. Formdan gelen yeni rütbe (veya rütbesizlik/null) ile kartı güncelliyoruz.
                operationClaim.OperationClaimId = userUpdateForAdminDto.OperationClaimId;
                await _userOperationClaimRepository.UpdateAsync(operationClaim);
            }
            else
            {
                if (userUpdateForAdminDto.OperationClaimId.HasValue)
                {
                    // Durum B: Adamın önceden kartı YOKTU. Eğer Admin formdan yeni bir rütbe seçmişse, adama sıfırdan bir yetki kartı basıyoruz.
                    UserOperationClaim userOperationClaim = new UserOperationClaim
                    {
                        UserId = existingUser.Id,
                        OperationClaimId = userUpdateForAdminDto.OperationClaimId
                    };
                    await _userOperationClaimRepository.AddAsync(userOperationClaim);
                }
            }
            return new SuccessResult("Kullanıcı başarıyla güncellendi.");
        }

        public async Task<IResult> UpdateMyProfileAsync(int userId, UserProfileUpdateDto userProfileUpdateDto)
        {
            userProfileUpdateDto.Email = userProfileUpdateDto.Email.Trim().ToLower();
            var existingUser = await _userRepository.GetAsync(x => x.Id == userId);
            if (existingUser == null)
            {
                return new ErrorResult("Güncellenecek kullanıcı bulunamadı!");
            }

            IResult? result = BusinessRules.Run(await CheckIfEmailExistsForUpdateAsync(userProfileUpdateDto.Email, existingUser.Id));
            if (result != null)
            {
                return result;
            }

            _mapper.Map(userProfileUpdateDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
            return new SuccessResult("Kullanıcı başarıyla güncellendi.");
        }



        /// <summary>
        /// Sistem içi yetkilendirme (AuthManager) süreçlerinde kullanılmak üzere, kullanıcının veritabanındaki rollerini (OperationClaims) getirir.
        /// Dikkat: Bu metot dışarıya (API'ye) açık değildir, DTO yerine çıplak Entity ile çalışır.
        /// </summary>

        public async Task<IDataResult<List<OperationClaim>>> GetClaimsAsync(User user)
        {
            // Depocunun o özel GetClaims metodunu çağırıp adamın rollerini alıyoruz.
            var claims = await _userRepository.GetClaimsAsync(user);
            return new SuccessDataResult<List<OperationClaim>>(claims, "Kullancı yetkileri başarıyla getirildi.");
        }


        public async Task<IResult> AddAsync(User user)
        {
            // Senior Vizyonu: Burada neden Validation (Kapı Memuru) veya AutoMapper yok?
            // Çünkü bu metodu sadece AuthManager çağıracak. AuthManager zaten kapıda şifre kurallarına baktı, 
            // DTO'yu User'a çevirdi, şifreyi Hash'ledi. Burada tekrar kontrol yaparsak kodu tekrar etmiş (Spagetti) oluruz.
            // O yüzden direkt ameleyle (Repository) depoya yolluyoruz!

            await _userRepository.AddAsync(user);
            return new SuccessResult("Kullancı güvenli bir şekilde sisteme eklendi.");
        }

        public async Task<IResult> CheckIfUserExistsAsync(int id)
        {
            bool existingUser = await _userRepository.AnyAsync(x => x.Id == id);
            if (existingUser)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu kullanıcı sistemde bulunamadı!");
        }

        public async Task<IResult> CheckIfEmailExistsAsync(string email)
        {
            bool existingEmail = await _userRepository.AnyAsync(x => x.Email == email, ignoreQueryFilters: true);
            if (existingEmail)
            {
                return new ErrorResult("Bu e-posta adresi zaten kayıtlı! Lütfen başka deneyiniz.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfEmailExistsForUpdateAsync(string email, int currentUserId)
        {
            bool isExist = await _userRepository.AnyAsync(x => x.Email == email && x.Id != currentUserId, ignoreQueryFilters: true);
            if (isExist)
            {
                return new ErrorResult("Bu e-posta adresi zaten kayıtlı! Lütfen başka deneyiniz.");
            }
            return new SuccessResult();
        }

        public async Task<IDataResult<User>> GetByMailAsync(string email)
        {
            var user = await _userRepository.GetAsync(u => u.Email == email, ignoreQueryFilters: true);
            if (user == null)
            {
                return new ErrorDataResult<User>("Bu e-posta adresine sahip kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<User>(user);
        }

        public async Task<IDataResult<User>> GetByIdForAuthAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<User>("Bu ID'ye sahip kullanıcı bulunamadı.");
            }
            return new SuccessDataResult<User>(user);
        }

        public async Task<IResult> UpdateForAuthAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
            return new SuccessResult("Şifre başarıyla güncellendi.");
        }
    }
}
