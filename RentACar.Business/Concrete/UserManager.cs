using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Business.Rules;
using RentACar.Core.Exceptions;
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
        private readonly UserBusinessRules _userBusinessRules;

        public UserManager(IUserRepository userRepository, IMapper mapper, UserBusinessRules userBusinessRules)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userBusinessRules = userBusinessRules;
        }

        public async Task<IResult> CheckIfUserExistsAsync(int userId)
        {
            return await _userBusinessRules.CheckIfUserExistsAsync(userId);
        }

        public async Task<IResult> DeleteMyAccountAsync(int id)
        {
            var existingUser = await _userRepository.GetAsync(x => x.Id == id);
            if (existingUser == null)
            {
                throw new BusinessException("Silinecek kullanıcı bulunamadı.");
            }
            await _userRepository.DeleteAsync(existingUser);
            return new SuccessResult("Kullanıcı başarıyla silindi.");
        }

        public async Task<IDataResult<List<UserResultDto>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            var sortedUsers = users.OrderByDescending(x => x.CreatedDate).ToList();
            var userDtos = _mapper.Map<List<UserResultDto>>(sortedUsers);
            return new SuccessDataResult<List<UserResultDto>>(userDtos, "Kullanıcılar başarıyla listelendi.");
        }

        public async Task<IDataResult<UserResultDto>> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BusinessException("Kullanıcı bulunamadı.");
            }

            var userDto = _mapper.Map<UserResultDto>(user);
            return new SuccessDataResult<UserResultDto>(userDto, "Kullancı başarıyla getirildi.");
        }

        public async Task<IDataResult<UserResultDto>> GetMyProfile(int id)
        {
            var user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null)
            {
                throw new BusinessException("Profil bulunamadı.");
            }

            var userDto = _mapper.Map<UserResultDto>(user);
            return new SuccessDataResult<UserResultDto>(userDto, "Profil başarıyla getirildi.");
        }

        public async Task<IResult> UpdateMyProfileAsync(int userId, UserProfileUpdateDto userProfileUpdateDto)
        {
            userProfileUpdateDto.Email = userProfileUpdateDto.Email.Trim().ToLower();
            var existingUser = await _userRepository.GetAsync(x => x.Id == userId);
            if (existingUser == null)
            {
                throw new BusinessException("Güncellenecek kullanıcı bulunamadı!");
            }

            IResult? result = BusinessRules.Run(
                await _userBusinessRules.CheckIfEmailExistsForUpdateAsync(userProfileUpdateDto.Email, existingUser.Id)
            );
            if (result != null)
            {
                throw new BusinessException(result.Message ?? "İş kurallarında beklenmeyen bir hata oluştu!");
            }

            _mapper.Map(userProfileUpdateDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
            return new SuccessResult("Kullanıcı başarıyla güncellendi.");
        }
    }
}
