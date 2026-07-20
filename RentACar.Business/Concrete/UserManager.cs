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
    }
}
