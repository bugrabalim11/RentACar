using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.Core.Utilities.Security.Hashing;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.UserDtos;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.Business.Concrete
{
    public class AdminUserManager : IAdminUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IAdminUserOperationClaimService _adminUserOperationClaimService;
        private readonly IMapper _mapper;
        private readonly UserBusinessRules _userBusinessRules;

        public AdminUserManager(IUserRepository userRepository, IUserOperationClaimService userOperationClaimService, IAdminUserOperationClaimService adminUserOperationClaimService, UserBusinessRules userBusinessRules, IMapper mapper)
        {
            _userRepository = userRepository;
            _adminUserOperationClaimService = adminUserOperationClaimService;
            _userOperationClaimService = userOperationClaimService;
            _userBusinessRules = userBusinessRules;
            _mapper = mapper;
        }

        public async Task<IDataResult<int>> CreateForAdminAsync(UserCreateByAdminDto userCreateForAdminDto)
        {
            // 1. İş Kuralı Kontrolü: Bu e-posta daha önce alınmış mı diye güvenlik kurallarımıza soruyoruz.
            userCreateForAdminDto.Email = userCreateForAdminDto.Email.Trim().ToLower();
            IResult? result = BusinessRules.Run(
                await _userBusinessRules.CheckIfEmailExistsAsync(userCreateForAdminDto.Email)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
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
                UserOperationClaimCreateDto userOperationClaimCreateDto = new UserOperationClaimCreateDto
                {
                    UserId = user.Id,
                    OperationClaimId = userCreateForAdminDto.OperationClaimId.Value
                };
                await _adminUserOperationClaimService.AddAsync(userOperationClaimCreateDto);
                return new SuccessDataResult<int>(userOperationClaimCreateDto.UserId, "Kullanıcı başarıyla eklendi ve rütbe ataması yapıldı.");
            }

            return new SuccessDataResult<int>(user.Id, "Kullanıcı başarıyla eklendi.");
        }

        public async Task<IResult> UpdateForAdminAsync(UserUpdateByAdminDto userUpdateForAdminDto)
        {
            // 1. Kimlik Kontrolü: Güncellenmek istenen adam gerçekten veritabanında (depoda) var mı?
            userUpdateForAdminDto.Email = userUpdateForAdminDto.Email.Trim().ToLower();
            var existingUser = await _userRepository.GetAsync(x => x.Id == userUpdateForAdminDto.Id);
            if (existingUser == null)
            {
                throw new BusinessException("Güncellenecek kullanıcı bulunamadı.");
            }
            IResult? result = BusinessRules.Run(
                await _userBusinessRules.CheckIfEmailExistsForUpdateAsync(userUpdateForAdminDto.Email, userUpdateForAdminDto.Id)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            // 3. Kimlik Kartını Güncelleme: Dışarıdan gelen formdaki (DTO) yeni bilgileri, veritabanından çektiğimiz gerçek nesnenin üzerine yazıyoruz.
            // : Map(Kaynak, Hedef)
            _mapper.Map(userUpdateForAdminDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);

            // 4. Yetki Kartı Operasyonunu Zimmet Defteri Departmanına Devrediyoruz (Clean Code!)
            await _adminUserOperationClaimService.SetUserClaimAsync(existingUser.Id, userUpdateForAdminDto.OperationClaimId);
            return new SuccessResult("Kullanıcı başarıyla güncellendi.");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingUser = await _userRepository.GetAsync(x => x.Id == id);
            if (existingUser == null)
            {
                throw new BusinessException("Silinecek kullanıcı bulunamadı.");
            }
            await _userRepository.DeleteAsync(existingUser);
            return new SuccessResult("Kullanıcı başarıyla silindi.");
        }

        public async Task<IResult> RestoreAsync(int id)
        {
            var deletedUser = await _userRepository.GetAsync(x => x.Id == id && x.IsDeleted == true, ignoreQueryFilters: true);
            if (deletedUser == null)
            {
                throw new BusinessException("Geri Getirilecek kullanıcı bulunamadı.");
            }
            deletedUser.IsDeleted = false;
            deletedUser.DeletedDate = null;
            await _userRepository.UpdateAsync(deletedUser);
            return new SuccessResult("Kullanıcı başarıyla geri getirildi.");
        }

        public async Task<IDataResult<List<UserResultByAdminDto>>> GetAllForAdminAsync()
        {
            var users = await _userRepository.GetAllAsync(ignoreQueryFilters: true);
            var sortedUsers = users.OrderByDescending(x => x.CreatedDate).ToList();
            var userDtos = _mapper.Map<List<UserResultByAdminDto>>(sortedUsers);
            return new SuccessDataResult<List<UserResultByAdminDto>>(userDtos, "Kullanıcılar başarıyla listelendi.");
        }

        public async Task<IDataResult<UserUpdateByAdminDto>> GetByIdForUpdateAsync(int id)
        {
            // 1. Kullanıcıyı bul
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BusinessException("Kullanıcı bulunamadı.");
            }

            var userDto = _mapper.Map<UserUpdateByAdminDto>(user);

            // 2. DOĞRU ŞEFİ, DOĞRU METOTLA ÇAĞIR! (GetByIdAsync DEĞİL!)
            var claimResult = await _userOperationClaimService.GetUpdateDtoByUserIdAsync(id);

            // 3. Şef başarılı döndüyse (Yani adamın gerçekten bir rütbe kartı varsa)
            if (claimResult.Success)
            {
                // DİKKAT: .Data.Id DEĞİL, .Data.OperationClaimId !!!
                userDto.OperationClaimId = claimResult.Data.OperationClaimId;
            }
            else
            {
                // Adamın rütbesi yoksa hata fırlatma, sadece null bırak. Formda "Rütbesiz" görünsün.
                userDto.OperationClaimId = null;
            }

            return new SuccessDataResult<UserUpdateByAdminDto>(userDto, "Kullancı başarıyla getirildi.");
        }
    }
}
