using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Business;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserManager(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IResult> AddAsync(UserAddDto userAddDto)
        {
            userAddDto.Email = userAddDto.Email.Trim().ToLower();
            IResult? result = BusinessRules.Run(await CheckIfEmailExistsAsync(userAddDto.Email));
            if (result != null)
            {
                return result;
            }

            var user = _mapper.Map<User>(userAddDto);
            user.Status = true;
            await _userRepository.AddAsync(user);
            return new SuccessResult("Kullanıcı başarıyla eklendi");
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var existingUser = await _userRepository.GetAsync(x => x.Id == id);
            if (existingUser == null)
            {
                return new ErrorResult("Silinecek kullanıcı bulunamadı.");
            }

            existingUser.Status = false;
            await _userRepository.UpdateAsync(existingUser);
            return new SuccessResult("Kullanıcı başarıyla silindi.");
        }

        public async Task<IDataResult<List<UserListDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var userDtos = _mapper.Map<List<UserListDto>>(users);
            return new SuccessDataResult<List<UserListDto>>(userDtos, "Kullanıcılar başarıyla listelendi.");
        }

        public async Task<IDataResult<UserListDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                return new ErrorDataResult<UserListDto>("Kullanıcı bulunamadı.");
            }

            var userDto = _mapper.Map<UserListDto>(user);
            return new SuccessDataResult<UserListDto>(userDto, "Kullancı başarıyla getirildi.");
        }

        public async Task<IResult> UpdateAsync(UserUpdateDto userUpdateDto)
        {
            userUpdateDto.Email = userUpdateDto.Email.Trim().ToLower();
            IResult? result = BusinessRules.Run(await CheckIfEmailExistsForUpdateAsync(userUpdateDto.Email, userUpdateDto.Id));
            if (result != null)
            {
                return result;
            }

            var existingUser = await _userRepository.GetAsync(x => x.Id == userUpdateDto.Id);
            if (existingUser == null)
            {
                return new ErrorResult("Güncellenecek kullanıcı bulunamadı.");
            }

            // : Map(Kaynak, Hedef)
            _mapper.Map(userUpdateDto, existingUser);
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



        /// <summary>
        /// Sistem içi giriş (Login) operasyonlarında (AuthManager), kullanıcının kimliğini
        /// ve şifre hash'ini doğrulamak amacıyla e-posta adresi üzerinden tarama yapar.
        /// </summary>
        public async Task<IDataResult<User>> GetByMailAsync(string email)
        {
            email = email.Trim().ToLower();
            var user = await _userRepository.GetAsync(x => x.Email == email);
            if (user == null)
            {
                return new ErrorDataResult<User>("Bu e-posta adresine sahip kullanıcı bulunamadı.");
            }

            return new SuccessDataResult<User>(user, "Kullanıcı başarıyla bulundu.");
        }

        public async Task<IResult> AddAsync(User user)
        {
            // Senior Vizyonu: Burada neden Validation (Kapı Memuru) veya AutoMapper yok?
            // Çünkü bu metodu sadece AuthManager çağıracak. AuthManager zaten kapıda şifre kurallarına baktı, 
            // DTO'yu User'a çevirdi, şifreyi Hash'ledi. Burada tekrar kontrol yaparsak kodu tekrar etmiş (Spagetti) oluruz.
            // O yüzden direkt ameleyle (Repository) depoya yolluyoruz!

            await _userRepository.AddAsync(user);
            return new SuccessResult("Kullancı güvenli vir şekilde sisteme eklendi.");
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
            bool existingEmail = await _userRepository.AnyAsync(x => x.Email == email);
            if (existingEmail)
            {
                return new ErrorResult("Bu e-posta adresi zaten kayıtlı! Lütfen başka deneyiniz.");
            }
            return new SuccessResult();
        }

        private async Task<IResult> CheckIfEmailExistsForUpdateAsync(string email, int currentUserId)
        {
            bool isExist = await _userRepository.AnyAsync(x => x.Email == email && x.Id != currentUserId);
            if (isExist)
            {
                return new ErrorResult("Bu e-posta adresi zaten kayıtlı! Lütfen başka deneyiniz.");
            }
            return new SuccessResult();
        }
    }
}
