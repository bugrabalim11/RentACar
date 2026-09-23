using RentACar.Business.Abstract;
using RentACar.Core.Utilities.Results;
using RentACar.DataAccess.Abstract;

namespace RentACar.Business.Concrete
{
    public class ReferenceCheckService : IReferenceCheckService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRentalRepository _rentalRepository;

        public ReferenceCheckService(IBrandRepository brandRepository, IColorRepository colorRepository, ICarRepository carRepository, IUserRepository userRepository, IRentalRepository rentalRepository)
        {
            _brandRepository = brandRepository;
            _colorRepository = colorRepository;
            _carRepository = carRepository;
            _userRepository = userRepository;
            _rentalRepository = rentalRepository;
        }

        public async Task<IResult> CheckIfBrandExistsAsync(int brandId)
        {
            bool existingBrand = await _brandRepository.AnyAsync(x => x.Id == brandId);
            if (existingBrand)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu marka sistemde bulunamadı!");
        }

        public async Task<IResult> CheckIfColorExistsAsync(int colorId)
        {
            bool existingColor = await _colorRepository.AnyAsync(x => x.Id == colorId);
            if (existingColor)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu renk sistemde bulunamadı!");
        }

        public async Task<IResult> CheckIfColorIsUsedByAnyCarAsync(int colorId)
        {
            bool existingCar = await _carRepository.AnyAsync(x => x.ColorId == colorId);
            if (!existingCar)
            {
                return new SuccessResult();
            }
            return new ErrorResult("Bu renk sistemdeki araçlar tarafından kullanıldığı için silinemez!");
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
