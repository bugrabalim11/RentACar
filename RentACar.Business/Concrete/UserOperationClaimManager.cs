using AutoMapper;
using RentACar.Business.Abstract;
using RentACar.Core.Exceptions;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;
using RentACar.Dtos.UserOperationClaimDtos;

namespace RentACar.Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userOperationClaimRepository;
        private readonly IMapper _mapper;

        public UserOperationClaimManager(IUserOperationClaimRepository userOperationClaimRepository, IMapper mapper)
        {
            _userOperationClaimRepository = userOperationClaimRepository;
            _mapper = mapper;
        }       

        public async Task<IDataResult<List<UserOperationClaimResultDto>>> GetAllAsync()
        {
            var userOperationClaims = await _userOperationClaimRepository.GetAllAsync();
            var sortedUserOperationClaims = userOperationClaims.OrderByDescending(x => x.CreatedDate).ToList();
            var userOperationClaimDtos = _mapper.Map<List<UserOperationClaimResultDto>>(sortedUserOperationClaims);
            return new SuccessDataResult<List<UserOperationClaimResultDto>>(userOperationClaimDtos, "Tüm kullanıcı yetkileri listelendi.");
        }

        public async Task<IDataResult<UserOperationClaimResultDto>> GetByIdAsync(int id)
        {
            var userOperationClaim = await _userOperationClaimRepository.GetAsync(x => x.Id == id);
            if (userOperationClaim == null)
            {
                throw new BusinessException("Belirtilen yetki ataması bulunamadı.");
            }

            var userOperationClaimDto = _mapper.Map<UserOperationClaimResultDto>(userOperationClaim);
            return new SuccessDataResult<UserOperationClaimResultDto>(userOperationClaimDto, "Yetki ataması başarıyla getirildi.");
        }

        public async Task<IDataResult<List<UserOperationClaimDetailDto>>> GetMyOperationClaimsAsync(int userId)
        {
            var myOperationClaims = await _userOperationClaimRepository.GetMyOperationClaimsAsync(userId);
            // Bir Junior refleksi şudur: "Acaba result == null diye kontrol etmeli miyim?"
            //Senior Mimar der ki: Hayır! Entity Framework'te ToListAsync() metodu eğer veritabanında
            //hiçbir kayıt bulamazsa null dönmez; içi boş bir liste (Örn: []) döner. Bu yüzden null kontrolü yapmana gerek yoktur.
            return new SuccessDataResult<List<UserOperationClaimDetailDto>>(myOperationClaims, "Size ait yetkiler başarıyla getirildi.");
        }

        public async Task<IDataResult<List<UserOperationClaimDetailDto>>> GetClaimDetailsAsync()
        {
            var claimDetails = await _userOperationClaimRepository.GetClaimDetailsAsync();
            return new SuccessDataResult<List<UserOperationClaimDetailDto>>(claimDetails, "Kullanıcı yetkileri detaylı olarak listelendi.");
        }

        public async Task<IDataResult<UserOperationClaimUpdateDto>> GetUpdateDtoByUserIdAsync(int userId)
        {
            var existingClaim = await _userOperationClaimRepository.GetAsync(x => x.UserId == userId);
            if (existingClaim == null)
            {
                return new ErrorDataResult<UserOperationClaimUpdateDto>("Kullanıcıya ait rütbe bulunamadı!");
            }
            var claimDto = _mapper.Map<UserOperationClaimUpdateDto>(existingClaim);
            return new SuccessDataResult<UserOperationClaimUpdateDto>(claimDto, "Yetki formu getirildi.");
        }
    }
}
