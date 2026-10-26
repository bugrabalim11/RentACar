using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class ReferenceCheckService : IReferenceCheckService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRentalRepository _rentalRepository;
        private readonly IOperationClaimRepository _operationClaimRepository;

        public ReferenceCheckService(IUserRepository userRepository, IRentalRepository rentalRepository, IOperationClaimRepository operationClaimRepository)
        {
            _userRepository = userRepository;
            _rentalRepository = rentalRepository;
            _operationClaimRepository = operationClaimRepository;
        }

        public async Task<IResult> CheckIfOfficeHasRentalsAsync(int officeId)
        {
            var checkRental = await _rentalRepository.AnyAsync(x => x.PickUpOfficeId == officeId || x.DropOffOfficeId == officeId);
            if (!checkRental)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Ofise ait kiralama işlemleri mevcut, bu yüzden silinemez!");
        }

        public async Task<IResult> CheckIfOperationClaimExistsAsync(int operationClaimId)
        {
            bool existOperationClaim = await _operationClaimRepository.AnyAsync(x => x.Id == operationClaimId);
            if (!existOperationClaim)
            {
                return new ErrorResult("Böyle bir yetki sistemde bulunamadı!");
            }
            return new SuccessResult();
        }

        public async Task<IResult> CheckIfUserExistsAsync(int userId)
        {
            bool existingUser = await _userRepository.AnyAsync(x => x.Id == userId);
            if (existingUser)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu kullanıcı sistemde bulunamadı!");
        }
    }
}
