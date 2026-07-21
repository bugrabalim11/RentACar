using AutoMapper;
using FluentValidation;
using RentACar.Business.Abstract;
using RentACar.Core.Entities.Concrete;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.UserDtos;

namespace RentACar.Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<UserAddDto> _addValidator;
        private readonly IValidator<UserUpdateDto> _updateValidator;
        public UserManager(IUserRepository userRepository, IMapper mapper, IValidator<UserAddDto> addValidator, IValidator<UserUpdateDto> updateValidator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
        }

        public async Task<IResult> AddAsync(UserAddDto userAddDto)
        {
            var validationResult = await _addValidator.ValidateAsync(userAddDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
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

            await _userRepository.DeleteAsync(existingUser);
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
            var validationResult = await _updateValidator.ValidateAsync(userUpdateDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
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
            var claims =await _userRepository.GetClaimsAsync(user);
            return new SuccessDataResult<List<OperationClaim>>(claims, "Kullancı yetkileri başarıyla getirildi.");
        }



        /// <summary>
        /// Sistem içi giriş (Login) operasyonlarında (AuthManager), kullanıcının kimliğini
        /// ve şifre hash'ini doğrulamak amacıyla e-posta adresi üzerinden tarama yapar.
        /// </summary>
        public async Task<IDataResult<User>> GetByMailAsync(string email)
        {
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
    }
}
